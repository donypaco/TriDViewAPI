using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TriDViewAPI.DTO;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Controllers
{
    //[Authorize("JwtPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {   private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService) { _storeService = storeService; }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            var store = await _storeService.GetStoreById(id);
            if (store == null)
                return NotFound();
            return Ok(store);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllActiveStores()
        {
            var store = await _storeService.GetAllActiveStores();
            if (store == null)
                return NotFound();
            return Ok(store);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _storeService.GetStoreById(id);
            if (store == null)
                return NotFound();

            await _storeService.DeleteStore(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreDTO storeDto)
        {
            if (storeDto == null || id != storeDto.Id)
                return BadRequest("Invalid store data");

            var existingStore = await _storeService.GetStoreById(id);
            if (existingStore == null)
                return NotFound();

            await _storeService.UpdateStore(storeDto);

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> AddStore([FromBody] StoreDTO storeDto)
        {
            if (storeDto == null)
                return BadRequest("Invalid store data");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);
            
            await _storeService.AddStore(storeDto, userId);

            return NoContent();
        }

    }
}
