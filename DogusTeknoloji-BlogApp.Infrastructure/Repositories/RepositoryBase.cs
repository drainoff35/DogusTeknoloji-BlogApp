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
    public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity not found");
            }
            _dbSet.Remove(entity);
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity not found");
            }
            return entity;
        }
        public async Task UpdateAsync(TKey id, TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Entity not found");
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
    }

}
