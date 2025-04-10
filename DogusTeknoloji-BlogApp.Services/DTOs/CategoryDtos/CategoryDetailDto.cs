using DogusTeknoloji_BlogApp.Services.DTOs.PostDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.CategoryDtos
{
    public class CategoryDetailDto
    {
        public string Name { get; set; } = null!;
        public List<PostDetailDto> Posts { get; set; } = new List<PostDetailDto>();
    }
}
