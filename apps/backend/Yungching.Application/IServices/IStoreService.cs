using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.DTOs;

namespace Yungching.Application.IServices
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDto>> GetAllAsync();
    }
}
