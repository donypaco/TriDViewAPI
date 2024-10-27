using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriDViewAPI.Services;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogService _logService;
        public ProductController(IProductService productService, ILogService logService)
        {
            _productService = productService;
            _logService = logService;
        }

        [HttpGet("/{storeId}")]
        public async Task<IActionResult> GetAllStoreProducts(int storeId)
        {
            var products = await _productService.GetAllStoreProducts(storeId);
            if (!products.Any())
                return NotFound();
            return Ok(products);
        }
    }
}
