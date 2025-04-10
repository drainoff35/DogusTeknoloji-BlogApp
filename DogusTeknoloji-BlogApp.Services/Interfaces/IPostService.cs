using DogusTeknoloji_BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Interfaces
{
    public interface IPostService : IServiceBase<Post, int>
    {
        Task<List<Post>> GetPostsByCategoryIdAsync(int categoryId);
        Task<Post> GetPostWithCommentsAsync(int postId);
        Task<List<Post>> GetAllWithCategoriesAsync();
    }
}
