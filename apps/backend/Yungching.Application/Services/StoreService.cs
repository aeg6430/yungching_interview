using Microsoft.Extensions.Logging;
using System.Net;
using Yungching.Application.Contracts.Stores;
using Yungching.Application.DTOs;
using Yungching.Application.IServices;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.Entities.Stores;
using Yungching.Infrastructure.IRepositories;


namespace Yungching.Application.Services
{
    public  class StoreService : IStoreService
    {
        private readonly TransactionContext _context;
        private readonly ILogger<StoreService> _logger;
        private readonly IStoreRepository _storeRepository;

        public StoreService(
            TransactionContext context,
            ILogger<StoreService> logger,
            IStoreRepository storeRepository
         )
        {
            _context = context;
            _logger = logger;
            _storeRepository = storeRepository;
        }

        public async Task AddStoreAsync(AddStoreRequest storeRequest)
        {
            try 
            {
                _context.Begin();
                var request = new StoreEntity
                {
                    StoreId = Guid.NewGuid(),
                    Name = storeRequest.Name,
                    Latitude = storeRequest.Latitude,
                    Longitude = storeRequest.Longitude,
                    Address = storeRequest.Address,
                    BusinessHours = storeRequest.BusinessHours,
                };

                await _storeRepository.AddStoreAsync(request, _context.Connection, _context.Transaction);
                _context.Commit();
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "AddStoreAsync Exception occurred");
            }
        }

        public async Task<IEnumerable<StoreDto>> GetAllAsync()
        {
            var entities = await _storeRepository.GetAllAsync();

            var dtos = entities.Select(entity => new StoreDto
            {
                StoreId = entity.StoreId,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Address = entity.Address,
                BusinessHours = entity.BusinessHours,
            }).ToList();

            return dtos; 
        }

        public async Task UpdateStoreAsync(UpdateStoreRequest updateStore)
        {
            try
            {
                _context.Begin();

                var storeCriteria = new StoreCriteria
                {
                    StoreId = updateStore.StoreId,
                };
                var existingEntity = await _storeRepository.GetByIdAsync(storeCriteria);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Update failed: Store not found {StoreId}", updateStore.StoreId);
                }

                existingEntity.Name = updateStore.Name;
                existingEntity.Latitude = updateStore.Latitude;
                existingEntity.Longitude = updateStore.Longitude;
                existingEntity.Address = updateStore.Address;
                existingEntity.BusinessHours = updateStore.BusinessHours;

                await _storeRepository.UpdateStoreAsync(existingEntity, _context.Connection, _context.Transaction);
                _context.Commit();
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "UpdateStoreAsync Exception occurred");
            }
        }

        public async Task RemoveStoreAsync(RemoveStoreRequest storeRequest)
        {
            try
            {
                _context.Begin();

                var request = new StoreCriteria
                {
                    StoreId = storeRequest.StoreId,
                };

                await _storeRepository.RemoveStoreAsync(request, _context.Connection, _context.Transaction);
                _context.Commit();
            }
            catch (Exception e)
            {
                _context.Rollback();
                _logger.LogError(e, "RemoveStoreAsync Exception occurred");
            }
        }
    }
}
