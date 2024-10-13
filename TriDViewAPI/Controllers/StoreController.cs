using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TriDViewAPI.DTO;
using TriDViewAPI.Services;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Controllers
{
    //[Authorize("JwtPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {   private readonly IStoreService _storeService;
        private readonly ILogService _logService;
        public StoreController(IStoreService storeService, ILogService logService) 
        { 
            _storeService = storeService; 
            _logService = logService;
        }

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
            try
            {
                var store = await _storeService.GetStoreById(id);
                if (store == null)
                    return NotFound();

                await _storeService.DeleteStore(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.LogError("UserController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreDTO storeDto)
        {
            try
            {

            if (storeDto == null || id != storeDto.Id)
                return BadRequest("Invalid store data");

            var existingStore = await _storeService.GetStoreById(id);
            if (existingStore == null)
                return NotFound();

            await _storeService.UpdateStore(storeDto);

            return NoContent();
            }
            catch (Exception ex)
            {
                _logService.LogError("UserController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddStore([FromBody] StoreDTO storeDto)
        {
            try
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
            catch (Exception ex)
            {
                _logService.LogError("UserController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
