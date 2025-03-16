using Microsoft.EntityFrameworkCore;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDTO>> GetAllRoles()
        {
            return await _context.Roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                RoleName = r.RoleName
            }).ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }

    }
}
