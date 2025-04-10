using Microsoft.AspNetCore.Mvc;
using DogusTeknoloji_BlogApp.Services.DTOs.UserDtos;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogusTeknoloji_BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    createUserDto.RegisterDate = DateTime.Now;
                    var result = _userService.CreateUser(createUserDto);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Kayıt işlemi başarıyla gerçekleşti. Giriş yapabilirsiniz.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu. Lütfen bilgilerinizi kontrol edin.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Beklenmeyen bir hata oluştu: {ex.Message}");
                }
            }
            return View(createUserDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(SignInDto signInDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _userService.SignIn(signInDto);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Giriş başarılı.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Giriş işlemi başarısız. Lütfen bilgilerinizi kontrol edin.");
                    }
                }
                catch (KeyNotFoundException)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                }
            }
            return View(signInDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {

            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userProfile = await _userService.GetUserProfileAsync(userId);
                return View(userProfile);
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _userService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
