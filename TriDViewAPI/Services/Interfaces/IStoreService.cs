using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Services.Interfaces
{
    public interface IStoreService
    {
        Task<Store> GetStoreById(int id);
        Task<IEnumerable<StoreDTO>> GetAllActiveStores();
        Task DeleteStore(int id);
        Task UpdateStore(StoreDTO store);
        Task AddStore(StoreDTO storeDTO, int userId);
    }
}
