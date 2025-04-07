using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Core.Entities
{
    public class AppUser:IdentityUser<Guid>
    {
        public DateTime RegisterDate { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
