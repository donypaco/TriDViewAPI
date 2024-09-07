using TaskManagementSystem.Data;
using TriDViewAPI.Data;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class LogService : ILogService
    {

        private readonly ApplicationDbContext _dbContext;

        public LogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogException(HttpContext context, Exception ex)
        {
            using (var scope = context.RequestServices.CreateScope())
            {
                _dbContext.Logs.Add(new Log
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Level = "Error",
                    Message = $"Exception: {ex.Message}",
                    Exception = ex.ToString()
                });

                _dbContext.SaveChanges();
            }
        }
    }

}
