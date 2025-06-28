using Yungching.Application.DTOs;
using Yungching.Application.IServices;
using Yungching.Infrastructure.IRepositories;


namespace Yungching.Application.Services
{
    public  class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;

        public StoreService(IStoreRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<StoreDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

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
     }
}
