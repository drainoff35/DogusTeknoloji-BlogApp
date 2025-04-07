using DogusTeknoloji_BlogApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Interfaces
{
    public interface IServiceBase<TEntity, TKey> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TKey id, TEntity entity);
        Task DeleteAsync(TKey id);
    }
}
