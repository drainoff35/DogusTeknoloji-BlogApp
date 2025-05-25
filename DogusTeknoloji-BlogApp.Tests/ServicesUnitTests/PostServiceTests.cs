using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DogusTeknoloji_BlogApp.Tests.ServicesUnitTests
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            // Setup mocks
            _mockPostRepository = new Mock<IPostRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            // Create instance of the service with mocked dependencies
            _postService = new PostService(
                _mockPostRepository.Object,
                _mockCategoryRepository.Object,
                _mockUnitOfWork.Object);
        }

        #region Base Service Methods

        [Fact]
        public async Task GetByIdAsync_ExistingPost_ReturnsPost()
        {
            // Arrange
            var postId = 1;
            var expectedPost = new Post 
            { 
                Id = postId, 
                Title = "Test Post",
                Content = "Test Content",
                CreatedAt = DateTime.Now
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(expectedPost);

            // Act
            var result = await _postService.GetByIdAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
            Assert.Equal("Test Post", result.Title);
            Assert.Equal("Test Content", result.Content);
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingPost_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingPostId = 999;

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(nonExistingPostId))
                .ReturnsAsync((Post)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _postService.GetByIdAsync(nonExistingPostId));

            _mockPostRepository.Verify(repo => repo.GetByIdAsync(nonExistingPostId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Post 1", Content = "Content 1", CreatedAt = DateTime.Now.AddDays(-2) },
                new Post { Id = 2, Title = "Post 2", Content = "Content 2", CreatedAt = DateTime.Now.AddDays(-1) },
                new Post { Id = 3, Title = "Post 3", Content = "Content 3", CreatedAt = DateTime.Now }
            };

            _mockPostRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            _mockPostRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ValidPost_AddsSuccessfully()
        {
            // Arrange
            var newPost = new Post
            {
                Title = "New Post",
                Content = "New Content",
                CreatedAt = DateTime.Now
            };

            _mockPostRepository.Setup(repo => repo.AddAsync(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _postService.AddAsync(newPost);

            // Assert
            _mockPostRepository.Verify(repo => repo.AddAsync(newPost), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidPost_UpdatesSuccessfully()
        {
            // Arrange
            var postId = 1;
            var existingPost = new Post
            {
                Id = postId,
                Title = "Existing Post",
                Content = "Existing Content",
                CreatedAt = DateTime.Now.AddDays(-1)
            };
            var updatedPost = new Post
            {
                Id = postId,
                Title = "Updated Post",
                Content = "Updated Content",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(existingPost);

            _mockPostRepository.Setup(repo => repo.UpdateAsync(postId, It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _postService.UpdateAsync(postId, updatedPost);

            // Assert
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
            _mockPostRepository.Verify(repo => repo.UpdateAsync(postId, updatedPost), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingPost_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingPostId = 999;
            var updatedPost = new Post
            {
                Id = nonExistingPostId,
                Title = "Updated Post",
                Content = "Updated Content",
                CreatedAt = DateTime.Now
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(nonExistingPostId))
                .ReturnsAsync((Post)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _postService.UpdateAsync(nonExistingPostId, updatedPost));

            _mockPostRepository.Verify(repo => repo.GetByIdAsync(nonExistingPostId), Times.Once);
            _mockPostRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Post>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingPost_DeletesSuccessfully()
        {
            // Arrange
            var postId = 1;
            var existingPost = new Post
            {
                Id = postId,
                Title = "Post to Delete",
                Content = "Content to Delete",
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(existingPost);

            _mockPostRepository.Setup(repo => repo.DeleteAsync(postId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _postService.DeleteAsync(postId);

            // Assert
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
            _mockPostRepository.Verify(repo => repo.DeleteAsync(postId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingPost_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingPostId = 999;

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(nonExistingPostId))
                .ReturnsAsync((Post)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _postService.DeleteAsync(nonExistingPostId));

            _mockPostRepository.Verify(repo => repo.GetByIdAsync(nonExistingPostId), Times.Once);
            _mockPostRepository.Verify(repo => repo.DeleteAsync(nonExistingPostId), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        #endregion

        #region PostService Specific Methods

        [Fact]
        public async Task GetAllWithCategoriesAsync_ReturnsAllPostsWithCategories()
        {
            // Arrange
            var category1 = new Category { Id = 1, Name = "Category 1" };
            var category2 = new Category { Id = 2, Name = "Category 2" };
            
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Post 1", Content = "Content 1", CategoryId = 1, Category = category1 },
                new Post { Id = 2, Title = "Post 2", Content = "Content 2", CategoryId = 1, Category = category1 },
                new Post { Id = 3, Title = "Post 3", Content = "Content 3", CategoryId = 2, Category = category2 }
            };

            _mockPostRepository.Setup(repo => repo.GetAllWithCategoriesAsync())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetAllWithCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Category 1", result[0].Category.Name);
            Assert.Equal("Category 2", result[2].Category.Name);
            _mockPostRepository.Verify(repo => repo.GetAllWithCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPostsByCategoryIdAsync_ExistingCategory_ReturnsFilteredPosts()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Test Category" };
            
            var posts = new List<Post>
            {
                new Post { Id = 1, Title = "Post 1", CategoryId = categoryId },
                new Post { Id = 2, Title = "Post 2", CategoryId = categoryId }
            };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockPostRepository.Setup(repo => repo.GetPostsByCategoryIdAsync(categoryId))
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsByCategoryIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, post => Assert.Equal(categoryId, post.CategoryId));
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockPostRepository.Verify(repo => repo.GetPostsByCategoryIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetPostsByCategoryIdAsync_NonExistingCategory_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = 999;

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(nonExistingCategoryId))
                .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _postService.GetPostsByCategoryIdAsync(nonExistingCategoryId));

            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(nonExistingCategoryId), Times.Once);
            _mockPostRepository.Verify(repo => 
                repo.GetPostsByCategoryIdAsync(nonExistingCategoryId), Times.Never);
        }

        [Fact]
        public async Task GetPostWithCommentsAsync_ExistingPost_ReturnsPostWithComments()
        {
            // Arrange
            var postId = 1;
            var post = new Post 
            { 
                Id = postId, 
                Title = "Test Post", 
                Content = "Test Content"
            };
            
            var postWithComments = new Post
            {
                Id = postId,
                Title = "Test Post",
                Content = "Test Content",
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, Text = "Comment 1", PostId = postId },
                    new Comment { Id = 2, Text = "Comment 2", PostId = postId }
                }
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _mockPostRepository.Setup(repo => repo.GetPostWithCommentsAsync(postId))
                .ReturnsAsync(postWithComments);

            // Act
            var result = await _postService.GetPostWithCommentsAsync(postId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.Id);
            Assert.NotNull(result.Comments);
            Assert.Equal(2, result.Comments.Count);
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
            _mockPostRepository.Verify(repo => repo.GetPostWithCommentsAsync(postId), Times.Once);
        }

        [Fact]
        public async Task GetPostWithCommentsAsync_NonExistingPost_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingPostId = 999;

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(nonExistingPostId))
                .ReturnsAsync((Post)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _postService.GetPostWithCommentsAsync(nonExistingPostId));

            _mockPostRepository.Verify(repo => repo.GetByIdAsync(nonExistingPostId), Times.Once);
            _mockPostRepository.Verify(repo => 
                repo.GetPostWithCommentsAsync(nonExistingPostId), Times.Never);
        }

        #endregion
    }
}
