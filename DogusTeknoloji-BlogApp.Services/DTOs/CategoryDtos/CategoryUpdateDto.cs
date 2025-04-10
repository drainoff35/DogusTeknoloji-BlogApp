using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İçerik boş bırakılamaz.")]
        public string Name { get; set; } = null!;
    }
}
