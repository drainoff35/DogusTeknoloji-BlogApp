using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.PostDtos
{
    public class PostCreateDto
    {
        [Required(ErrorMessage = "Konu başlığı boş bırakılamaz.")]
        [MaxLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
        [MaxLength(1000, ErrorMessage = "İçerik en fazla 1000 karakter olabilir.")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Kategori boş bırakılamaz.")]
        public int CategoryId { get; set; }

        [Display(Name = "Resim")]
        public IFormFile? Image { get; set; }
    }
}
