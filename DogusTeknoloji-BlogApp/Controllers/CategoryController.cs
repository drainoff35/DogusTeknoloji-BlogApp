using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.DTOs.CategoryDtos;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusTeknoloji_BlogApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = _mapper.Map<Category>(categoryDto);
                    await _categoryService.AddAsync(category);
                    await _unitOfWork.CommitAsync();
                    TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                }
            }
            return View(categoryDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryUpdateDto>(category);
            return View(categoryDto);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CategoryUpdateDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = _mapper.Map<Category>(categoryDto);
                    await _categoryService.UpdateAsync(id, category);
                    await _unitOfWork.CommitAsync();
                    TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound("İlgili kategori bulunamadı.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Kategori güncellenirken bir hata oluştu: {ex.Message}");
                }
            }
            return View(categoryDto);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                return Json(new { success = true, message = "Kategori başarıyla silindi." });
            }
            catch (KeyNotFoundException)
            {
                return Json(new { success = false, message = "Kategori bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
