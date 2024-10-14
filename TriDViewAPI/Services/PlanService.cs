
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class PlanService: IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ILogService _logService;
        public PlanService( IPlanRepository planRepository, IConfiguration configuration, ILogService logService)
        {
            _planRepository = planRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<PlanDTO>> GetAllPlans()
        {
            try
            {
                return await _planRepository.GetAllPlansAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError("PlanService", ex.ToString());
            }
            return Enumerable.Empty<PlanDTO>();
            ;
        }
    }
}
