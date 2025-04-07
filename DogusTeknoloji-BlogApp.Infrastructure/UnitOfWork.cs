using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Infrastructure.Data;
using DogusTeknoloji_BlogApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Comments = new CommentRepository(_context);
            Categories = new CategoryRepository(_context);
            Posts = new PostRepository(_context);
        }

        public ICommentRepository Comments { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IPostRepository Posts { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
