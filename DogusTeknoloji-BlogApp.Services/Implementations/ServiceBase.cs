using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Implementations
{
    public class ServiceBase<TEntity, TKey> : IServiceBase<TEntity,TKey> where TEntity : BaseEntity
    {
        private readonly IRepositoryBase<TEntity, TKey> _repository;

        public ServiceBase(IRepositoryBase<TEntity, TKey> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<TEntity> GetByIdAsync(TKey id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(TKey id, TEntity entity)
        {
            await _repository.UpdateAsync(id, entity);
        }
    }

}
