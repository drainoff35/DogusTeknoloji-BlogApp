using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.DTOs.PostDtos;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DogusTeknoloji_BlogApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IPostService postService, ICategoryService categoryService,
            ICommentService commentService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _postService = postService;
            _categoryService = categoryService;
            _commentService = commentService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllWithCategoriesAsync();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostWithCommentsAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var postViewModel = _mapper.Map<PostDetailDto>(post);
            return View(postViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropdownAsync();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateDto postDto)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var post = _mapper.Map<Post>(postDto);
                    post.CreatedAt = DateTime.Now;
                    post.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    if (postDto.Image != null && postDto.Image.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(postDto.Image.FileName);
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posts");

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await postDto.Image.CopyToAsync(fileStream);
                        }

                        post.ImagePath = "/images/posts/" + uniqueFileName;
                    }

                    await _postService.AddAsync(post);
                    await _unitOfWork.CommitAsync();
                    TempData["SuccessMessage"] = "Yazı başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Yazı oluşturulurken bir hata oluştu: {ex.Message}");
                }
            }

            await PopulateCategoriesDropdownAsync();
            return View(postDto);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {

            var post = await _postService.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.UserId != currentUserId)
            {
                TempData["ErrorMessage"] = "Sadece kendi oluşturduğunuz postları düzenleyebilirsiniz.";
                return RedirectToAction("Index");
            }

            var postVM = _mapper.Map<PostUpdateDto>(post);
            postVM.ExistingImagePath = post.ImagePath;
            await PopulateCategoriesDropdownAsync(post.CategoryId);
            return View(postVM);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, PostUpdateDto postDto)
        {
            var originalPost = await _postService.GetByIdAsync(id);
            if (originalPost == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var post = _mapper.Map<Post>(postDto);
                    post.UserId = originalPost.UserId;
                    post.CreatedAt = originalPost.CreatedAt;

                    if (postDto.Image == null || postDto.Image.Length == 0)
                    {
                        post.ImagePath = originalPost.ImagePath;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(originalPost.ImagePath))
                        {
                            var oldImagePath = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                "wwwroot",
                                originalPost.ImagePath.TrimStart('/'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(postDto.Image.FileName);
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "posts");

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await postDto.Image.CopyToAsync(fileStream);
                        }

                        post.ImagePath = "/images/posts/" + uniqueFileName;
                    }

                    await _postService.UpdateAsync(id, post);
                    await _unitOfWork.CommitAsync();
                    TempData["SuccessMessage"] = "Post başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                }
            }

            await PopulateCategoriesDropdownAsync(postDto.CategoryId);
            return View(postDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _postService.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                return Json(new { success = true, message = "Yazı başarıyla silindi." });
            }
            catch (KeyNotFoundException)
            {
                return Json(new { success = false, message = "Silinecek yazı bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }



        [HttpGet]

        public async Task<IActionResult> ByCategory(int categoryId)
        {
            try
            {
                var posts = await _postService.GetPostsByCategoryIdAsync(categoryId);
                return View("Index", posts);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Kategori bulunamadı.");
            }
        }

        private async Task PopulateCategoriesDropdownAsync(int? selectedCategoryId = null)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
    }
}

