using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Infrastructure.Repositories
{
    public class PostRepository : RepositoryBase<Post, int>, IPostRepository
    {
        
        public PostRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Post>> GetAllWithCategoriesAsync()
        {
            return await _dbSet
        .Include(p => p.Category)
        .Include(p => p.Comments)
        .Include(p => p.User)
        .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
        .Include(p => p.Category)
        .Include(p => p.Comments)
        .Include(p => p.User)
        .Where(p => p.CategoryId == categoryId)
        .ToListAsync();
        }

        public async Task<Post> GetPostWithCommentsAsync(int postId)
        {
            return await _dbSet
        .Include(p => p.Comments)
            .ThenInclude(c => c.User)
        .Include(p => p.User)
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == postId);
        }
    }
}
