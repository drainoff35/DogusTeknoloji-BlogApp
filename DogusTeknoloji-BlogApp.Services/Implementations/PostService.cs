using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusTeknoloji_BlogApp.Services.Implementations
{
    public class PostService : ServiceBase<Post, int>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        public PostService(IPostRepository postRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : base(postRepository, unitOfWork)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<Post>> GetAllWithCategoriesAsync()
        {
            return await _postRepository.GetAllWithCategoriesAsync();
        }

        public async Task<List<Post>> GetPostsByCategoryIdAsync(int categoryId)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Kategori bulunamadı.");
            }
            return await _postRepository.GetPostsByCategoryIdAsync(categoryId);
        }

        public async Task<Post> GetPostWithCommentsAsync(int postId)
        {
            var existingPost = await _postRepository.GetByIdAsync(postId);
            if (existingPost == null)
            {
                throw new KeyNotFoundException("Post bulunamadı.");
            }
            return await _postRepository.GetPostWithCommentsAsync(postId);
        }
    }
}
