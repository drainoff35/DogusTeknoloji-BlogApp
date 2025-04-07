using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Models.ViewModels.CategoryViewModels;
using DogusTeknoloji_BlogApp.Services.Interfaces;
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

            // GET: Category
            public async Task<IActionResult> Index()
            {
                List<Category> categories = await _categoryService.GetAllAsync();
                return View(categories);
            }


            // GET: Category/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Category/Create
            [HttpPost]
            public async Task<IActionResult> Create(CategoryCreateViewModel categoryVM)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var category = _mapper.Map<Category>(categoryVM);
                        await _categoryService.AddAsync(category);
                        await _unitOfWork.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Something went wrong: {ex.Message}");
                    }
                }
                return View(categoryVM);
            }

            // GET: Category/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                try
                {
                    var category = await _categoryService.GetByIdAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    var categoryVM = _mapper.Map<CategoryUpdateViewModel>(category);
                    return View(categoryVM);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }

            // POST: Category/Edit/5
            [HttpPost]
            public async Task<IActionResult> Edit(int id, CategoryUpdateViewModel categoryVM)
            {
                if (id != categoryVM.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var category = _mapper.Map<Category>(categoryVM);
                        await _categoryService.UpdateAsync(id, category);
                        await _unitOfWork.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (KeyNotFoundException)
                    {
                        return NotFound();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Something went wrong: {ex.Message}");
                    }
                }
                return View(categoryVM);
            }

            // GET: Category/Delete/5
            public async Task<IActionResult> Delete(int id)
            {
                try
                {
                    var category = await _categoryService.GetByIdAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    return View(category);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }

            // POST: Category/Delete/5
            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                try
                {
                    await _categoryService.DeleteAsync(id);
                    await _unitOfWork.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Delete), new { id, errorMessage = ex.Message });
                }
            }
        }
 }
