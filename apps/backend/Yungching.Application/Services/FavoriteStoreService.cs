using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yungching.Application.Contracts.FavoriteStores;
using Yungching.Application.DTOs;
using Yungching.Application.IServices;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.FavoriteStores;
using Yungching.Infrastructure.IRepositories;

namespace Yungching.Application.Services
{
    public class FavoriteStoreService : IFavoriteStoreService
    {
        private readonly TransactionContext _context;
        private readonly ILogger<FavoriteStoreService> _logger;
        private readonly IFavoriteStoreRepository _favoriteStoreRepository;
        private readonly IStoreRepository _storeRepository;
        
        public FavoriteStoreService(
            TransactionContext context,
            ILogger<FavoriteStoreService> logger,
            IFavoriteStoreRepository favoriteStoreRepository,
            IStoreRepository storeRepository
         )
        {
            _context = context;
            _logger = logger;
            _favoriteStoreRepository = favoriteStoreRepository;
            _storeRepository = storeRepository;
        }

        public async Task AddAsync(AddFavoriteStoreRequest favoriteStoreRequest)
        {
            try {
                _context.Begin();
                var request = new FavoriteStoreCriteria
                {
                    AppUserId = favoriteStoreRequest.UserId,
                    StoreId = favoriteStoreRequest.StoreId
                };
                await _favoriteStoreRepository.AddFavoriteStoreAsync(request, _context.Connection, _context.Transaction);
                _context.Commit();
            } 
            catch(Exception e) 
            {
                _context.Rollback();
                _logger.LogError(e, "AddFavoriteStoreAsync Exception occurred");
            }
        }

        public async Task<IEnumerable<StoreDto>> GetFavoriteStoresAsync(GetFavoriteStoreRequest request)
        {
            IEnumerable<Guid> storeIds;

            if (request.StoreId != Guid.Empty)
            {

                var storeRequest = new FavoriteStoreCriteria
                {
                    AppUserId = request.UserId,
                    StoreId = request.StoreId,           
                };
                var favorite = await _favoriteStoreRepository.GetStoreAsync(storeRequest);
                storeIds = new[] { favorite.StoreId };
            }
            else
            {
                var favorites = await _favoriteStoreRepository.GetStoresAsync(request.UserId);
                storeIds = favorites.Select(f => f.StoreId).ToList();
            }

            var storeEntities = await _storeRepository.GetByIdsAsync(storeIds);

            return storeEntities.Select(store => new StoreDto
            {
                StoreId = store.StoreId,
                Name = store.Name,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                Address = store.Address,
                BusinessHours = store.BusinessHours
            });
        }

        public async Task RemoveAsync(RemoveFavoriteStoreRequest favoriteStoreRequest)
        {
            try 
            {
                _context.Begin();
                var request = new FavoriteStoreCriteria
                {
                    AppUserId = favoriteStoreRequest.UserId,
                    StoreId = favoriteStoreRequest.StoreId,
                };
                await _favoriteStoreRepository.RemoveFavoriteStoreAsync(request, _context.Connection, _context.Transaction);
                _context.Commit();
            } 
            catch (Exception e) 
            {
                _context.Rollback();
                _logger.LogError(e, "RemoveFavoriteStoreAsync Exception occurred");
            }   
        }
    }
}
