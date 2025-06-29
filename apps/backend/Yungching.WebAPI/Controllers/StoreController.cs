using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yungching.Application.Contracts.Stores;
using Yungching.Application.IServices;

namespace Yungching.WebAPI.Controllers
{
    [ApiController]
    [Route("stores")]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _storeService;

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService storeService
        )
        {
            _logger = logger;
            _storeService = storeService;
        }
        [HttpPost]
        public async Task<IActionResult> AddStore([FromQuery] AddStoreRequest request)
        {
            await _storeService.AddStoreAsync(request);
            return Ok(new { message = "Store added." });
        }
        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _storeService.GetAllAsync();
            return Ok(stores);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStore([FromQuery] UpdateStoreRequest request)
        {
            await _storeService.UpdateStoreAsync(request);
            return Ok(new { message = "Store updated." });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveStore([FromQuery] RemoveStoreRequest request)
        {
            await _storeService.RemoveStoreAsync(request);
            return Ok(new { message = "Store removed." });
        }
    }
}
