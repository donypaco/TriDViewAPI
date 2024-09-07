using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Services.Interfaces
{
    public interface IUserService
    {
        string GenerateJwtToken(User user);
        Task<string> Register(RegisterModel model);
        Task<string> Login(LoginModel model);
        Task<List<RoleDTO>> GetRoles();
    }
}
