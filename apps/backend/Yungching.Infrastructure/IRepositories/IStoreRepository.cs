using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Infrastructure.Entities.Stores;

namespace Yungching.Infrastructure.IRepositories
{
    public interface IStoreRepository
    {
        Task AddStoreAsync(StoreEntity store, IDbConnection connection, IDbTransaction transaction);
        Task<IEnumerable<StoreEntity>> GetAllAsync();
        Task<StoreEntity> GetByIdAsync(StoreCriteria storeCriteria);
        Task<IEnumerable<StoreEntity>> GetByIdsAsync(IEnumerable<Guid> storeIds);
        Task UpdateStoreAsync(StoreEntity store, IDbConnection connection, IDbTransaction transaction);
        Task RemoveStoreAsync(StoreCriteria storeCriteria, IDbConnection connection, IDbTransaction transaction);
    }
}
