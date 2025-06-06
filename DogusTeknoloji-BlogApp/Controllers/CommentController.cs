﻿using AutoMapper;
using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.DTOs.CommentDtos;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogusTeknoloji_BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IPostService postService, IMapper mapper)
        {
            _commentService = commentService;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(int postId, CommentCreateDto commentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var comment = _mapper.Map<Comment>(commentDto);
                    comment.PostId = postId;
                    comment.CreatedAt = DateTime.Now;
                    comment.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    await _commentService.AddAsync(comment);

                    TempData["SuccessMessage"] = "Yorumunuz başarıyla eklendi.";
                    return RedirectToAction("Details", "Post", new { id = postId });
                }

                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Yorum eklenirken hata oluştu: {ex.Message}";
                }
            }

            return RedirectToAction("Details", "Post", new { id = postId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                Console.WriteLine($"Comment UserId: {comment.UserId}");
                Console.WriteLine($"Current UserId: {userId}");

                // Sadece kendi yorumunu silebilsin
                if (comment.UserId != userId)
                {
                    return Json(new { success = false, message = "Bu yorumu silmek için yetkiniz yok." });
                }

                int postId = comment.PostId;
                await _commentService.DeleteAsync(id);

                return Json(new { success = true, message = "Yorum başarıyla silindi.", postId = postId });
            }
            catch (KeyNotFoundException)
            {
                return Json(new { success = false, message = "Yorum bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Yorum silinirken hata oluştu: {ex.Message}" });
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Sadece kendi yorumunu düzenleyebilsin
                if (comment.UserId != userId)
                {
                    TempData["ErrorMessage"] = "Bu yorumu düzenlemek için yetkiniz yok.";
                    return RedirectToAction("Details", "Post", new { id = comment.PostId });
                }

                var commentDto = _mapper.Map<CommentUpdateDto>(comment);
                ViewBag.PostId = comment.PostId;
                return View(commentDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Yorum bulunamadı.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CommentUpdateDto commentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var originalComment = await _commentService.GetByIdAsync(id);
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    // Sadece kendi yorumunu düzenleyebilirsin
                    if (originalComment.UserId != userId)
                    {
                        TempData["ErrorMessage"] = "Bu yorumu düzenlemek için yetkiniz yok.";
                        return RedirectToAction("Details", "Post", new { id = originalComment.PostId });
                    }

                    _mapper.Map(commentDto, originalComment);
                    await _commentService.UpdateAsync(id, originalComment);

                    TempData["SuccessMessage"] = "Yorum başarıyla güncellendi.";
                    return RedirectToAction("Details", "Post", new { id = originalComment.PostId });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound("Yorum bulunamadı.");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Yorum güncellenirken hata oluştu: {ex.Message}";
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(commentDto);
        }
    }
}
