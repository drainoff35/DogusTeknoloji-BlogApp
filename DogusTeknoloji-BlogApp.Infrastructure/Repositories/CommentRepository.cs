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
    public class CommentRepository : RepositoryBase<Comment, int>, ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        }
    }
}
