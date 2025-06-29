using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _storeService.GetAllAsync();
            return Ok(stores);
        }
    }
}
