using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Yungching.Application.IServices;
using Yungching.Application.Contracts.FavoriteStores;
using Microsoft.AspNetCore.Authorization;

namespace Yungching.WebAPI.Controllers
{
    [ApiController]
    [Route("favorites/stores")]
    [Authorize]
    public class FavoriteStoresController : ControllerBase
    {
        private readonly IFavoriteStoreService _favoriteStoreService;

        public FavoriteStoresController(IFavoriteStoreService favoriteStoreService)
        {
            _favoriteStoreService = favoriteStoreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites([FromQuery] GetFavoriteStoreRequest request)
        {
            var stores = await _favoriteStoreService.GetFavoriteStoresAsync(request);
            return Ok(stores);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromQuery] AddFavoriteStoreRequest request)
        {
            await _favoriteStoreService.AddAsync(request);
            return Ok(new { message = "Store added to favorites." });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFavorite([FromQuery ] RemoveFavoriteStoreRequest request)
        {
            await _favoriteStoreService.RemoveAsync(request);
            return Ok(new { message = "Store removed from favorites." });
        }
    }
}
