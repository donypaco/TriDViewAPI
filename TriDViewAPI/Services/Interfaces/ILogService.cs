namespace TriDViewAPI.Services.Interfaces
{
    public interface ILogService
    {
        Task LogError(string title, string message);
        Task LogInfo(string title, string message);
        Task LogWarning(string title, string message);
    }
}
