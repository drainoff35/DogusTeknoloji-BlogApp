using DogusTeknoloji_BlogApp.Services.DTOs.CommentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.DTOs.PostDtos
{
    public class PostDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public List<CommentDetailDto> Comments { get; set; } = new List<CommentDetailDto>();
    }
}
