using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Domain.ValueObjects;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Users;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly DapperContext _context;

        public UserRepository(
            ILogger<UserRepository> logger,
            DapperContext context
        )
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> IsEmailExistedAsync(Email email)
        {
            string sql = @"
            SELECT 
                1
            FROM app_user 
            WHERE email = @Email
            ";
            var parameters = new
            {
                Email = email.Value,
            };
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, parameters);
                return result.HasValue;
            }
        }
        public async Task<UserEntity?> GetUserByIdAsync(UserId userId)
        {
            var sql = @"
             SELECT app_user_id AS AppUserId, email AS Email, 
                name AS Name, password AS Password,
                created_time AS CreatedTime, 
                modified_time AS ModifiedTime
             FROM app_user WHERE app_user_id = @AppUserId";
            var parameters = new
            {
                AppUserId = userId.Value,
            };
            using (var connection = _context.CreateConnection())
            {

                var result = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, parameters);

                return result;
            }
        }

        public async Task<UserEntity?> GetUserByEmailAsync(Email email)
        {
            var sql = @"
             SELECT app_user_id AS AppUserId, email AS Email,
                name AS Name, password AS Password,
                created_time AS CreatedTime,
                modified_time AS ModifiedTime
             FROM app_user WHERE email = @Email";

            var parameters = new
            {
                Email = email.Value,
            };
            using (var connection = _context.CreateConnection()) { 

             var result = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, parameters);

            return result;
           }
        }
        public async Task CreateUserAsync(UserEntity user, IDbConnection connection, IDbTransaction transaction)
        {
            string insert = @"
            INSERT INTO app_user (
                app_user_id, email, name,password,
                created_time
            ) VALUES (
                @AppUserId, @Email, @Name,@Password,
                @CreatedTime
            )";
            var parameters = new UserEntity
            {
                AppUserId = user.AppUserId,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                CreatedTime = user.CreatedTime,
            };

            await connection.ExecuteAsync(insert, parameters, transaction);
        }

        public async Task UpdateUserAsync(UserEntity user, IDbConnection connection, IDbTransaction transaction)
        {
            string update = @"
            UPDATE app_user
            SET 
                name = @Name,
                password = @Password,
                modified_time = @ModifiedTime
            WHERE app_user_id = @AppUserId";


            var parameters = new UserEntity
            {
                Name = user.Name,
                Password = user.Password,
                ModifiedTime = user.ModifiedTime,
                AppUserId = user.AppUserId
            };

            await connection.ExecuteAsync(update, parameters, transaction);
        }

        public async Task DeleteUserAsync(UserId userId, IDbConnection connection, IDbTransaction transaction)
        {
            string delete = @"
            DELETE FROM app_user
            WHERE app_user_id = @UserId";

            var parameters = new
            {
                UserId = userId.Value
            };

            await connection.ExecuteAsync(delete, parameters, transaction);
        }
    }
}
