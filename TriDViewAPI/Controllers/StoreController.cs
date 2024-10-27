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
        private readonly ILogService _logService;
        public StoreController(IStoreService storeService, ILogService logService) 
        { 
            _storeService = storeService; 
            _logService = logService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            try {
                var store = await _storeService.GetStoreById(id);
                if (store == null)
                    return NotFound();
                return Ok(store);
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllActiveStores()
        {
            try
            {
                var store = await _storeService.GetAllActiveStores();
                if (store == null)
                    return NotFound();
                return Ok(store);
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
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
                await _logService.LogError("StoreController", ex.ToString());
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
                await _logService.LogError("StoreController", ex.ToString());
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
                await _logService.LogError("StoreController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterStore([FromForm] StoreDTO storeDto)
        {
            try
            {
                if (storeDto == null || storeDto.formFile == null)
                    return BadRequest("Invalid store data");

                await _storeService.RegisterStore(storeDto, storeDto.formFile);

                return NoContent();
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("ConfirmRegistration/{storeId}")]
        public async Task<IActionResult> ConfirmRegistration(int storeId)
        {
            try
            {
                if (storeId <= 0)  // Changed to <= to catch 0 and negative IDs
                    return BadRequest("Invalid store ID");

                await _storeService.ConfirmRegistration(storeId);

                return NoContent(); // 204 No Content response
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreController", ex.ToString());
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
