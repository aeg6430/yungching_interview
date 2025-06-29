using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.DTOs.Users;
using Yungching.Domain.Models;

namespace Yungching.Application.Auth
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto?> RegisterUserAsync(UserRegisterDto registerDto);

        Task<UserLoginResponseDto?> LoginUserAsync(UserLoginDto loginDto);

        Task<User?> UpdateUserAsync(User updatedUser);

        Task<bool> IsEmailExistedAsync(string email);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
