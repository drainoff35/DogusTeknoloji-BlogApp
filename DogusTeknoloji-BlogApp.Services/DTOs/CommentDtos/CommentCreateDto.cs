﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.CommentDtos
{
    public class CommentCreateDto
    {
        [Required(ErrorMessage = "Yorum boş bırakılamaz.")]
        [MaxLength(500, ErrorMessage = "Yorum en fazla 500 karakter olabilir.")]
        public string Text { get; set; } = null!;
    }
}
