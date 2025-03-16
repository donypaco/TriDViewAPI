using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
