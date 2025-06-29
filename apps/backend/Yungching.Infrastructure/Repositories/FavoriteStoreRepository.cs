using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.FavoriteStores;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.Infrastructure.Repositories
{
    public class FavoriteStoreRepository : IFavoriteStoreRepository
    {
        private readonly ILogger<FavoriteStoreRepository> _logger;
        private readonly DapperContext _context;

        public FavoriteStoreRepository(
            ILogger<FavoriteStoreRepository> logger,
            DapperContext context
        )
        {
            _logger = logger;
            _context = context;
        }

        public async Task AddFavoriteStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = @"
            INSERT INTO favorite_store (
                app_user_id, store_id
            )
            VALUES (@AppUserId, @StoreId)
            ";
            var parameters = new FavoriteStoreCriteria
            {
                AppUserId = favoriteStoreCriteria.AppUserId,
                StoreId = favoriteStoreCriteria.StoreId,
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<FavoriteStoreEntity> GetStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria)
        {
            string query = @"
                SELECT 
                    app_user_id AS UserId, 
                    store_id AS StoreId
                FROM favorite_store
                WHERE 
                    app_user_id = @AppUserId
                AND store_id = @StoreId
            ";
            var parameters = new FavoriteStoreCriteria
            {
                AppUserId = favoriteStoreCriteria.AppUserId,
                StoreId = favoriteStoreCriteria.StoreId,
            };
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<FavoriteStoreEntity>(query, parameters);

                return result;
            }
        }

        public async Task<IEnumerable<FavoriteStoreEntity>> GetStoresAsync(Guid userId)
        {
            string query = @"
                SELECT 
                    app_user_id AS UserId, 
                    store_id AS StoreId
                FROM favorite_store
                WHERE 
                    app_user_id = @AppUserId
            ";
            var parameters = new 
            {
                AppUserId = userId
            };
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<FavoriteStoreEntity>(query, parameters);

                return result;
            }
        }

        public async Task RemoveFavoriteStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = @"
            DELETE FROM favorite_store
            WHERE app_user_id = @AppUserId 
            AND store_id = @StoreId
            ";
            var parameters = new FavoriteStoreCriteria
            {
                AppUserId = favoriteStoreCriteria.AppUserId,
                StoreId = favoriteStoreCriteria.StoreId,
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }
    }
}
