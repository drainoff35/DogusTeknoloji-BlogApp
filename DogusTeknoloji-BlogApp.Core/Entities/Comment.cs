using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Core.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        // Navigation properties
        public Post Post { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }
}
