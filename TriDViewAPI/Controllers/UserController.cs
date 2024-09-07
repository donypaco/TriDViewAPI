
using Microsoft.AspNetCore.Mvc;
using TriDViewAPI.DTO;
using TriDViewAPI.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    //[Authorize("JwtPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        public UserController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var token = _userService.Register(model);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return StatusCode(500, "An error occurred while registering.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                var token = _userService.Login(model);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("Roles")]
        public async Task<List<RoleDTO>> GetRoles()
        {
            try
            {
                return await _userService.GetRoles();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
