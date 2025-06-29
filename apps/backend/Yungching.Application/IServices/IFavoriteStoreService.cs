using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.Contracts.FavoriteStores;
using Yungching.Application.DTOs;

namespace Yungching.Application.IServices
{
    public interface IFavoriteStoreService
    {
         Task AddAsync(AddFavoriteStoreRequest request);
         Task<IEnumerable<StoreDto>> GetFavoriteStoresAsync(GetFavoriteStoreRequest request); 
         Task RemoveAsync(RemoveFavoriteStoreRequest request);
    }
}
