using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [MaxLength(40, ErrorMessage = "Kategori adı en fazla 40 karakter olabilir.")]
        public string Name { get; set; } = null!;
    }
}
