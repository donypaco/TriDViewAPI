using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TriDViewAPI.Data;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, ILogService logService,
            IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _logService = logService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<string> Register(RegisterModel model)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                    return "Email already registered!";

                var role = await _roleRepository.GetRoleById(model.RoleId);
                if (role == null)
                {
                    return "No role found!";
                }

                User user = new User
                {
                    UserName = model.Username,
                    Password = _userManager.PasswordHasher.HashPassword(null, model.Password),
                    Email = model.Email,
                    Role = role
                };
                await _userRepository.AddUserAsync(user);

                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                _logService.LogError("UserService", ex.ToString());
                throw;
            }
            return null;
        }
        public async Task<string> Login(LoginModel model)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    var result = (passwordVerificationResult == PasswordVerificationResult.Success);

                    if (result)
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        };
                        return GenerateJwtToken(user);

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logService.LogError("UserService", ex.ToString());
                throw;
            }
            return null;

        }
        public string GenerateJwtToken(User user)
        {
            try
            {

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"])
                };

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logService.LogError("UserService", ex.ToString());
                throw;
            }
            return null;
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            try
            {
                  return await _roleRepository.GetAllRoles();
            }
            catch (Exception ex)
            {
                _logService.LogError("UserService", ex.ToString());
                throw;
            }
            return null;
        }
    }
}
