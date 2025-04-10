using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.UserDtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string UserName { get; set; } = null!;
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
        public string Password { get; set; } = null!;
    }
}
