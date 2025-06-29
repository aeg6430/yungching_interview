using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.Contracts.Stores;
using Yungching.Application.DTOs;

namespace Yungching.Application.IServices
{
    public interface IStoreService
    {
        Task AddStoreAsync(AddStoreRequest request);
        Task<IEnumerable<StoreDto>> GetAllAsync();
        Task UpdateStoreAsync(UpdateStoreRequest request);
        Task RemoveStoreAsync(RemoveStoreRequest request);
    }
}
