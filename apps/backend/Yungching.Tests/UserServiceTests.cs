using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.Auth;
using Yungching.Application.DTOs.Users;
using Yungching.Application.Jwt;
using Yungching.Domain.Models;
using Yungching.Domain.ValueObjects;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Users;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
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
        public async Task RegisterUserAsync_Should_Return_Response_When_Successful()
        {
            var dto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "password",
                Name = "Tester"
            };

            _mockUserRepo.Setup(r => r.IsEmailExistedAsync(It.IsAny<Email>()))
                .ReturnsAsync(false);

            _mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<UserEntity>(), It.IsAny<System.Data.IDbConnection>(), It.IsAny<System.Data.IDbTransaction>()))
                .Returns(Task.CompletedTask);

            var result = await _service.RegisterUserAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(dto.Email, result.Email);
            Assert.AreEqual(dto.Name, result.Name);
        }

        [Test]
        public async Task RegisterUserAsync_Should_Return_Null_When_Email_Exists()
        {
            var dto = new UserRegisterDto
            {
                Email = "exists@example.com",
                Password = "password",
                Name = "User"
            };

            _mockUserRepo.Setup(r => r.IsEmailExistedAsync(It.IsAny<Email>()))
                .ReturnsAsync(true);

            var result = await _service.RegisterUserAsync(dto);

            Assert.IsNull(result);
        }

        [Test]
        public async Task LoginUserAsync_Should_Return_Token_When_Credentials_Valid()
        {
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserEntity
            {
                AppUserId = new UserId(Guid.NewGuid()),
                Email = new Email("login@example.com"),
                Name = "User",
                Password = hashedPassword
            };

            _mockUserRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<Email>()))
                .ReturnsAsync(user);

            _mockJwtToken.Setup(t => t.GenerateToken(It.IsAny<JwtTokenPayload>()))
                .Returns("mock-token");

            var result = await _service.LoginUserAsync(new UserLoginDto
            {
                Email = user.Email,
                Password = password
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual("mock-token", result.AuthToken);
        }

        [Test]
        public async Task LoginUserAsync_Should_Return_Null_When_Password_Wrong()
        {
            var user = new UserEntity
            {
                AppUserId = new UserId(Guid.NewGuid()),
                Email = new Email("user@example.com"),
                Name = "User",
                Password = BCrypt.Net.BCrypt.HashPassword("correct-password")
            };

            _mockUserRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<Email>()))
                .ReturnsAsync(user);

            var result = await _service.LoginUserAsync(new UserLoginDto
            {
                Email = user.Email,
                Password = "wrong-password"
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task IsEmailExistedAsync_Should_Return_True_If_Exists()
        {
            _mockUserRepo.Setup(r => r.IsEmailExistedAsync(It.IsAny<Email>()))
                .ReturnsAsync(true);

            var result = await _service.IsEmailExistedAsync("any@example.com");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteUserAsync_Should_Return_True_On_Success()
        {
            var userId = Guid.NewGuid();

            _mockUserRepo.Setup(r => r.DeleteUserAsync(It.IsAny<UserId>(), It.IsAny<System.Data.IDbConnection>(), It.IsAny<System.Data.IDbTransaction>()))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteUserAsync(userId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateUserAsync_Should_Return_Updated_User()
        {
            var userId = Guid.NewGuid();
            var updatedUser = new User
            {
                UserId = userId,
                Name = "Updated Name",
                Email = "updated@example.com",
                Password = "newpass"
            };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<UserId>()))
                .ReturnsAsync(new UserEntity
                {
                    AppUserId = new UserId(userId),
                    Email = new Email("old@example.com"),
                    Name = "Old",
                    Password = "oldhash"
                });

            _mockUserRepo.Setup(r => r.UpdateUserAsync(It.IsAny<UserEntity>(), It.IsAny<System.Data.IDbConnection>(), It.IsAny<System.Data.IDbTransaction>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateUserAsync(updatedUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.Name, result.Name);
            Assert.AreEqual(updatedUser.Email, result.Email);
        }
    }
}
