using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TriDViewAPI.Data;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services;
using TriDViewAPI.Services.Interfaces;

namespace TaskManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;

        public UserService(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, ILogService logService)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _logService = logService;
        }
        public async Task<string> Register(RegisterModel model)
        {
            try
            {
                var role = await _context.Roles.FindAsync(model.RoleId);
                if (role == null)
                {
                    return null;
                }

                User user = new User
                {
                    UserName = model.Username,
                    Password = _userManager.PasswordHasher.HashPassword(null, model.Password),
                    Email = model.Email,
                    Role = role
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

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
                var user = _context.Users.SingleOrDefault(u => u.UserName == model.Username);
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
                var roles = await _context.Roles.Select(r => new RoleDTO
                {
                    Id = r.Id,
                    RoleName = r.RoleName
                }).ToListAsync();

                return roles;
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
