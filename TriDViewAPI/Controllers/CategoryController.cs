using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            if (!categories.Any())
                return NotFound();
            return Ok(categories);
        }
    }
}