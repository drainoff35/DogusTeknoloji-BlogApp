using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Core.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        // Navigation properties
        public AppUser User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public List<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
