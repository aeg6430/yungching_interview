using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Stores;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ILogger<StoreRepository> _logger;
        private readonly DapperContext _context;

        public StoreRepository(
            ILogger<StoreRepository> logger,
            DapperContext context
        )
        {
            _logger = logger;
            _context = context;
        }

        public async Task AddStoreAsync(StoreEntity store, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = @"
            INSERT INTO store (
                store_id,name,address,
                latitude,longitude,business_hours
            )
            VALUES (
            @StoreId,@Name,@Address,
            @Latitude,@Longitude,@BusinessHours
            )
            ";
            var parameters = new StoreEntity
            {
                StoreId = store.StoreId,
                Name = store.Name,
                Address = store.Address,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                BusinessHours = store.BusinessHours,
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task<IEnumerable<StoreEntity>> GetAllAsync()
        {
            string sql = @"
            SELECT 
                store_id AS StoreId, 
                name AS Name,
                latitude AS Latitude,  
                longitude AS Longitude, 
                address AS Address, 
                business_hours AS BusinessHours
            FROM store 
            ";

            using (var connection = _context.CreateConnection())
            {
                var result  = await connection.QueryAsync<StoreEntity>( sql );

                return result;
            }
        }

        public async Task<StoreEntity> GetByIdAsync(StoreCriteria storeCriteria)
        {
            string sql = @"
               SELECT 
                    store_id AS StoreId, 
                    name AS Name,
                    latitude AS Latitude,  
                    longitude AS Longitude, 
                    address AS Address, 
                    business_hours AS BusinessHours
                FROM store
                WHERE store_id = @StoreId
                ";

            var parameters = new StoreCriteria
            {
                StoreId = storeCriteria.StoreId,
            };
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<StoreEntity>(sql, parameters);

                return result;
            }
        }

        public async Task<IEnumerable<StoreEntity>> GetByIdsAsync(IEnumerable<Guid> storeIds)
        {
            string sql = @"
           SELECT 
                    store_id AS StoreId, 
                    name AS Name,
                    latitude AS Latitude,  
                    longitude AS Longitude, 
                    address AS Address, 
                    business_hours AS BusinessHours
                FROM store
            WHERE store_id IN @StoreIds
            ";

            var parameters = new
            { 
                StoreIds = storeIds
            };
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<StoreEntity>(sql, parameters);

                return result;
            }
        }

        public async Task UpdateStoreAsync(StoreEntity store, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = @"
            UPDATE store 
            SET
                name = @Name,
                address = @Address,
                latitude = @Latitude,
                longitude = @Longitude,
                business_hours = @BusinessHours
            WHERE store_id = @StoreId
            ";
            var parameters = new StoreEntity
            {
                StoreId = store.StoreId,
                Name = store.Name,
                Address = store.Address,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                BusinessHours = store.BusinessHours,
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }

        public async Task RemoveStoreAsync(StoreCriteria storeCriteria, IDbConnection connection, IDbTransaction transaction)
        {
            string sql = @"
            DELETE FROM store 
            WHERE store_id = @StoreId
            ";
            var parameters = new StoreCriteria
            {
                StoreId = storeCriteria.StoreId,
            };
            await connection.ExecuteAsync(sql, parameters, transaction);
        }
    }
}
