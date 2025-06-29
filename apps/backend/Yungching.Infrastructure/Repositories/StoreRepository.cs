using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
