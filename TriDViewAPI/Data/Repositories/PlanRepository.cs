using Microsoft.EntityFrameworkCore;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories
{
    public class PlanRepository :IPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public PlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PlanDTO>> GetAllPlansAsync()
        {
            return await _context.Plans
                .Select(p => new PlanDTO
            {
                Id = p.Id,
                PlanName = p.PlanName
            }).ToListAsync();
        }
    }
}
