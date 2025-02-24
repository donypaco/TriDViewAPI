using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleDTO>> GetAllRoles();
        Task<Role> GetRoleById(int roleId);
    }
}
