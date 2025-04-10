using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.UserDtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        public string Password { get; set; } = null!;
    }
}
