using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yungching.Application.DTOs.Users;
using Yungching.Application.Jwt;
using Yungching.Domain.Models;
using Yungching.Domain.ValueObjects;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Users;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.Application.Auth
{
    public class UserService : IUserService
    {
        private readonly TransactionContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(
            TransactionContext context,
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IJwtTokenService jwtTokenService
        )
        {
            _context = context;
            _userRepository = userRepository;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserRegisterResponseDto?> RegisterUserAsync(UserRegisterDto registerDto)
        {
            try
            {
                _context.Begin();
                 var isExisted = await _userRepository.IsEmailExistedAsync(new Email(registerDto.Email));
                if (isExisted)
                {
                    _logger.LogWarning("Register failed: Email already exists - {Email}", registerDto.Email);
                    return null;
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var newUser = new UserEntity
                {
                    AppUserId = new UserId(Guid.NewGuid()),
                    Email = new Email(registerDto.Email),
                    Name = registerDto.Name,
                    Password = hashedPassword,
                    CreatedTime = DateTime.Now
                };

                await _userRepository.CreateUserAsync(newUser, _context.Connection, _context.Transaction);

                _context.Commit();
                return new UserRegisterResponseDto
                {
                    UserId = newUser.AppUserId,
                    Email = newUser.Email,
                    Name = newUser.Name
                };
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "RegisterUserAsync Exception occurred");
                return null;
            }
        }

        public async Task<UserLoginResponseDto?> LoginUserAsync(UserLoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(new Email(loginDto.Email));
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    return null;
                }
                var tokenPayload = new JwtTokenPayload
                {
                    UserId = user.AppUserId,
                    Email = user.Email,
                };
                return new UserLoginResponseDto
                {
                    UserId = user.AppUserId,
                    Email = user.Email,
                    Name = user.Name,
                    AuthToken = _jwtTokenService.GenerateToken(tokenPayload)
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LoginUserAsync Exception occurred");
                return null;
            }
        }

        public async Task<User?> UpdateUserAsync(User updatedUser)
        {
            try
            {
                _context.Begin();
                var existingEntity = await _userRepository.GetUserByIdAsync(new UserId(updatedUser.UserId));
                if (existingEntity == null)
                {
                    _logger.LogWarning("Update failed: User not found {UserId}", updatedUser.UserId);
                    return null;
                }

                existingEntity.Name = updatedUser.Name;
                existingEntity.Email = new Email(updatedUser.Email);
                if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                {
                    existingEntity.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
                }
                existingEntity.ModifiedTime = DateTime.Now;


                await _userRepository.UpdateUserAsync(existingEntity, _context.Connection, _context.Transaction);
                _context.Commit();

                _logger.LogInformation("User updated: {UserId}", updatedUser.UserId);

                return updatedUser; 
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "UpdateUserAsync Exception occurred");
                return null;
            }
        }

        public async Task<bool> IsEmailExistedAsync(string email)
        {
            try
            {
                return await _userRepository.IsEmailExistedAsync(new Email(email));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "IsEmailExistedAsync Exception occurred");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            try
            {
                _context.Begin();

                await _userRepository.DeleteUserAsync(new UserId(userId), _context.Connection, _context.Transaction);
                _context.Commit();

                _logger.LogInformation("User deleted: {UserId}", userId);
                return true;
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "DeleteUserAsync Exception occurred");
                return false;
            }
        }
    }
}
