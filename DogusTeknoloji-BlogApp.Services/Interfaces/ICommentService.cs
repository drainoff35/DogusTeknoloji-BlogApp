using DogusTeknoloji_BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Interfaces
{
    public interface ICommentService : IServiceBase<Comment, int>
    {
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
    }
}
