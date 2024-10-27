using Microsoft.AspNetCore.Mvc;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;
        private readonly ILogService _logService;
        public PlanController(IPlanService planService, ILogService logService)
        {
            _planService = planService;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _planService.GetAllPlans();
            if (!plans.Any())
                return NotFound();
            return Ok(plans);
        }
    }
}
