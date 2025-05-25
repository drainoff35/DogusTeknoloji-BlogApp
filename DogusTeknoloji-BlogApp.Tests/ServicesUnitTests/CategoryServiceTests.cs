using DogusTeknoloji_BlogApp.Core.Entities;
using DogusTeknoloji_BlogApp.Core.Interfaces.Repositories;
using DogusTeknoloji_BlogApp.Core.Interfaces.UnitOfWork;
using DogusTeknoloji_BlogApp.Services.Implementations;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DogusTeknoloji_BlogApp.Tests.ServicesUnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            // Setup mocks
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            // Create instance of the service with mocked dependencies
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingCategory_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var expectedCategory = new Category { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal("Test Category", result.Name);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingCategory_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = 999;

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(nonExistingCategoryId))
                .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _categoryService.GetByIdAsync(nonExistingCategoryId));

            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(nonExistingCategoryId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
                new Category { Id = 3, Name = "Category 3" }
            };

            _mockCategoryRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ValidCategory_AddsSuccessfully()
        {
            // Arrange
            var newCategory = new Category { Name = "New Category" };

            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _categoryService.AddAsync(newCategory);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.AddAsync(newCategory), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidCategory_UpdatesSuccessfully()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "Existing Category" };
            var updatedCategory = new Category { Id = categoryId, Name = "Updated Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(categoryId, It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _categoryService.UpdateAsync(categoryId, updatedCategory);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(categoryId, updatedCategory), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingCategory_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = 999;
            var updatedCategory = new Category { Id = nonExistingCategoryId, Name = "Updated Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(nonExistingCategoryId))
                .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _categoryService.UpdateAsync(nonExistingCategoryId, updatedCategory));

            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(nonExistingCategoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingCategory_DeletesSuccessfully()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "Category to Delete" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(categoryId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _categoryService.DeleteAsync(categoryId);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingCategory_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = 999;

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(nonExistingCategoryId))
                .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _categoryService.DeleteAsync(nonExistingCategoryId));

            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(nonExistingCategoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(nonExistingCategoryId), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}
