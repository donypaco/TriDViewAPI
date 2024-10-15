using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanDTO>> GetAllPlans();
    }
}
