using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.PostDtos
{
    public class PostUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Konu başlığı boş bırakılamaz.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Kategori boş bırakılamaz.")]
        public int CategoryId { get; set; }
    }
}
