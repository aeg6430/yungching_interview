using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Infrastructure.Entities.FavoriteStores;

namespace Yungching.Infrastructure.IRepositories
{
    public interface IFavoriteStoreRepository
    {
        Task AddFavoriteStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria, IDbConnection connection, IDbTransaction transaction);
        Task<FavoriteStoreEntity> GetStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria);
        Task<IEnumerable<FavoriteStoreEntity>> GetStoresAsync(Guid userId);
        Task RemoveFavoriteStoreAsync(FavoriteStoreCriteria favoriteStoreCriteria, IDbConnection connection, IDbTransaction transaction);
    }
}
