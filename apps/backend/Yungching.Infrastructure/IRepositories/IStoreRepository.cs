using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Infrastructure.Entities.Stores;

namespace Yungching.Infrastructure.IRepositories
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreEntity>> GetAllAsync();
        Task<IEnumerable<StoreEntity>> GetByIdsAsync(IEnumerable<Guid> storeIds);
    }
}
