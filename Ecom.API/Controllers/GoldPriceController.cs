using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldPriceController : ControllerBase
    {
        private readonly GoldPriceService _goldPriceService;

        public GoldPriceController(GoldPriceService goldPriceService)
        {
            _goldPriceService = goldPriceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var price = await _goldPriceService.GetGoldPriceAsync();
            return Ok(price);
        }
    }
}
