using TriDViewAPI.Data;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class LogService : ILogService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public LogService(ApplicationDbContext dbContext, IServiceScopeFactory scopeFactory)
        {
            _dbContext = dbContext;
            _scopeFactory = scopeFactory;
        }

        public async Task LogError(string title,string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _dbContext.Logs.Add(new Log
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Level = "Error",
                    Message = $"Exception: {message}",
                    Exception = message
                });;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task LogInfo(string title, string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _dbContext.Logs.Add(new Log
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Level = "Info",
                    Message = message,
                    Exception = null
                });

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task LogWarning(string title, string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _dbContext.Logs.Add(new Log
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Level = "Warning",
                    Message = message,
                    Exception = null
                });

                await _dbContext.SaveChangesAsync();
            }
        }

    }

}
