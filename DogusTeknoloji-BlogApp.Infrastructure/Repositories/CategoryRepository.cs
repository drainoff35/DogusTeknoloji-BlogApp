using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Infrastructure.Repositories
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
