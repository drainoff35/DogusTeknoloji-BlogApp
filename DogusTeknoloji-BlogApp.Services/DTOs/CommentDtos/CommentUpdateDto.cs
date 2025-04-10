using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.CommentDtos
{
    public class CommentUpdateDto
    {
        [Required(ErrorMessage = "Yorum boş bırakılamaz.")]
        public string Text { get; set; } = null!;
    }
}
