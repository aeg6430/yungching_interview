using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Domain.ValueObjects;
using Yungching.Infrastructure.Entities.Users;

namespace Yungching.Infrastructure.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExistedAsync(Email email);
        Task<UserEntity?> GetUserByIdAsync(UserId userId);
        Task<UserEntity?> GetUserByEmailAsync(Email email);
        Task CreateUserAsync(UserEntity user, IDbConnection connection, IDbTransaction transaction);
        Task UpdateUserAsync(UserEntity user, IDbConnection connection, IDbTransaction transaction);
        Task DeleteUserAsync(UserId userId, IDbConnection connection, IDbTransaction transaction);
    }
}
