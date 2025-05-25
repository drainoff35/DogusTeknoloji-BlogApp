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
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            // Setup mocks
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            // Create instance of the service with mocked dependencies
            _commentService = new CommentService(_mockCommentRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingComment_ReturnsComment()
        {
            // Arrange
            var commentId = 1;
            var expectedComment = new Comment
            {
                Id = commentId,
                Text = "Test Comment",
                CreatedAt = DateTime.Now
            };

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(commentId))
                .ReturnsAsync(expectedComment);

            // Act
            var result = await _commentService.GetByIdAsync(commentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(commentId, result.Id);
            Assert.Equal("Test Comment", result.Text);
            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(commentId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingComment_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCommentId = 999;

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(nonExistingCommentId))
                .ReturnsAsync((Comment)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _commentService.GetByIdAsync(nonExistingCommentId));

            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(nonExistingCommentId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllComments()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Text = "Comment 1", CreatedAt = DateTime.Now.AddDays(-2) },
                new Comment { Id = 2, Text = "Comment 2", CreatedAt = DateTime.Now.AddDays(-1) },
                new Comment { Id = 3, Text = "Comment 3", CreatedAt = DateTime.Now }
            };

            _mockCommentRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(comments);

            // Act
            var result = await _commentService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            _mockCommentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ValidComment_AddsSuccessfully()
        {
            // Arrange
            var newComment = new Comment
            {
                Text = "New Comment",
                CreatedAt = DateTime.Now
            };

            _mockCommentRepository.Setup(repo => repo.AddAsync(It.IsAny<Comment>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _commentService.AddAsync(newComment);

            // Assert
            _mockCommentRepository.Verify(repo => repo.AddAsync(newComment), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidComment_UpdatesSuccessfully()
        {
            // Arrange
            var commentId = 1;
            var existingComment = new Comment
            {
                Id = commentId,
                Text = "Existing Comment",
                CreatedAt = DateTime.Now.AddDays(-1)
            };
            var updatedComment = new Comment
            {
                Id = commentId,
                Text = "Updated Comment",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(commentId))
                .ReturnsAsync(existingComment);

            _mockCommentRepository.Setup(repo => repo.UpdateAsync(commentId, It.IsAny<Comment>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _commentService.UpdateAsync(commentId, updatedComment);

            // Assert
            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(commentId), Times.Once);
            _mockCommentRepository.Verify(repo => repo.UpdateAsync(commentId, updatedComment), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingComment_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCommentId = 999;
            var updatedComment = new Comment
            {
                Id = nonExistingCommentId,
                Text = "Updated Comment",
                CreatedAt = DateTime.Now
            };

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(nonExistingCommentId))
                .ReturnsAsync((Comment)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _commentService.UpdateAsync(nonExistingCommentId, updatedComment));

            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(nonExistingCommentId), Times.Once);
            _mockCommentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Comment>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingComment_DeletesSuccessfully()
        {
            // Arrange
            var commentId = 1;
            var existingComment = new Comment
            {
                Id = commentId,
                Text = "Comment to Delete",
                CreatedAt = DateTime.Now.AddDays(-3)
            };

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(commentId))
                .ReturnsAsync(existingComment);

            _mockCommentRepository.Setup(repo => repo.DeleteAsync(commentId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _commentService.DeleteAsync(commentId);

            // Assert
            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(commentId), Times.Once);
            _mockCommentRepository.Verify(repo => repo.DeleteAsync(commentId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingComment_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCommentId = 999;

            _mockCommentRepository.Setup(repo => repo.GetByIdAsync(nonExistingCommentId))
                .ReturnsAsync((Comment)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _commentService.DeleteAsync(nonExistingCommentId));

            _mockCommentRepository.Verify(repo => repo.GetByIdAsync(nonExistingCommentId), Times.Once);
            _mockCommentRepository.Verify(repo => repo.DeleteAsync(nonExistingCommentId), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}
