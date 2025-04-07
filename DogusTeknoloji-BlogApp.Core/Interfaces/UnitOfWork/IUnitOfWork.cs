using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICommentRepository Comments { get; }
        ICategoryRepository Categories { get; }
        IPostRepository Posts { get; }
        Task<int> CommitAsync();
    }
}
