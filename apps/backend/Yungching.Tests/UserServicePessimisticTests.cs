using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Yungching.Application.Auth;
using Yungching.Application.DTOs.Users;
using Yungching.Application.Jwt;
using Yungching.Domain.Models;
using Yungching.Domain.ValueObjects;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Users;
using Yungching.Infrastructure.IRepositories;
using NUnit.Framework;

namespace Yungching.UnitTests.Services
{
    [TestFixture]
    public class UserServicePessimisticTests
    {
        private Mock<TransactionContext> _mockContext;
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<ILogger<UserService>> _mockLogger;
        private Mock<IJwtTokenService> _mockJwtToken;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<TransactionContext>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _mockJwtToken = new Mock<IJwtTokenService>();

            _mockContext.Setup(c => c.Connection).Returns(Mock.Of<System.Data.IDbConnection>());
            _mockContext.Setup(c => c.Transaction).Returns(Mock.Of<System.Data.IDbTransaction>());

            _service = new UserService(
                _mockContext.Object,
                _mockUserRepo.Object,
                _mockLogger.Object,
                _mockJwtToken.Object
            );
        }

        [Test]
        public async Task RegisterUserAsync_Should_Return_Null_And_Rollback_On_Exception()
        {
            var dto = new UserRegisterDto
            {
                Email = "fail@example.com",
                Password = "password",
                Name = "Fail User"
            };

            _mockUserRepo.Setup(r => r.IsEmailExistedAsync(It.IsAny<Email>()))
                .ThrowsAsync(new Exception("DB error"));

            var result = await _service.RegisterUserAsync(dto);

            Assert.IsNull(result);
            _mockContext.Verify(c => c.Rollback(), Times.Once);
            LoggerVerifier.VerifyLogError(_mockLogger, Times.Once());
        }

        [Test]
        public async Task LoginUserAsync_Should_Return_Null_When_User_Not_Found()
        {
            _mockUserRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<Email>()))
                .ReturnsAsync((UserEntity?)null);

            var result = await _service.LoginUserAsync(new UserLoginDto
            {
                Email = "notfound@example.com",
                Password = "password"
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateUserAsync_Should_Return_Null_And_Rollback_When_User_Not_Found()
        {
            var updatedUser = new User
            {
                UserId = Guid.NewGuid(),
                Name = "Nonexistent",
                Email = "noone@example.com"
            };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<UserId>()))
                .ReturnsAsync((UserEntity?)null);

            var result = await _service.UpdateUserAsync(updatedUser);

            Assert.IsNull(result);
            _mockContext.Verify(c => c.Rollback(), Times.Once);
            LoggerVerifier.VerifyLogWarning(_mockLogger, Times.Once());
        }

        [Test]
        public async Task DeleteUserAsync_Should_Return_False_And_Rollback_On_Exception()
        {
            var userId = Guid.NewGuid();

            _mockUserRepo.Setup(r => r.DeleteUserAsync(
                    It.IsAny<UserId>(),
                    It.IsAny<System.Data.IDbConnection>(),
                    It.IsAny<System.Data.IDbTransaction>()))
                .ThrowsAsync(new Exception("DB delete error"));

            var result = await _service.DeleteUserAsync(userId);

            Assert.IsFalse(result);
            _mockContext.Verify(c => c.Rollback(), Times.Once);
            LoggerVerifier.VerifyLogError(_mockLogger, Times.Once());
        }

        [Test]
        public async Task IsEmailExistedAsync_Should_Return_False_And_LogError_On_Exception()
        {
            _mockUserRepo.Setup(r => r.IsEmailExistedAsync(It.IsAny<Email>()))
                .ThrowsAsync(new Exception("DB check error"));

            var result = await _service.IsEmailExistedAsync("fail@example.com");

            Assert.IsFalse(result);
            LoggerVerifier.VerifyLogError(_mockLogger, Times.Once());
        }
    }
}
