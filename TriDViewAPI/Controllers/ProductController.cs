using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TriDViewAPI.DTO;
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

        [HttpGet("{storeId}")]
        public async Task<IActionResult> GetAllStoreProducts(int storeId)
        {
            try
            {
                var products = await _productService.GetAllStoreProducts(storeId);
                if (!products.Any())
                    return NotFound();
                return Ok(products);
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                    return NotFound();

                await _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {
            try
            {

                if (productDto == null || id != productDto.ProductID)
                    return BadRequest("Invalid store data");

                var existingProduct = await _productService.GetProductById(id);
                if (existingProduct == null)
                    return NotFound();

                await _productService.UpdateProduct(productDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productDto, IFormFile image)
        {
            try
            {
                if (productDto == null)
                    return BadRequest("Invalid product data");
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized();
                }

                int userId = int.Parse(userIdClaim.Value);

                await _productService.AddProduct(productDto, userId, image);

                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
