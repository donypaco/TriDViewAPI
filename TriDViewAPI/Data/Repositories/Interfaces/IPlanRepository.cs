using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<PlanDTO>> GetAllPlansAsync();
    }
}
