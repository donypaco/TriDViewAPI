using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreDTO>> GetAllActiveStoresAsync();
        Task<Store> GetStoreByIdAsync(int id);
        Task AddStoreAsync(Store store);
        Task UpdateStoreAsync(Store store);
        Task DeleteStoreAsync(int id);
    }
}
