using Microsoft.AspNetCore.Identity;
using TriDViewAPI.Data;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class StoreService : IStoreService
    {
        //private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;

        public StoreService(IConfiguration configuration, UserManager<User> userManager, IStoreRepository storeRepository, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
        }
        public async Task<Store> GetStoreById(int id)
        {
            return await _storeRepository.GetStoreByIdAsync(id);
        }
        public async Task<IEnumerable<StoreDTO>> GetAllActiveStores()
        {
            return await _storeRepository.GetAllActiveStoresAsync();
        }
        public async Task DeleteStore(int id)
        {
            await _storeRepository.DeleteStoreAsync(id);
        }
        public async Task UpdateStore(StoreDTO storeDTO)
        {
            var store = await _storeRepository.GetStoreByIdAsync(storeDTO.Id);
            if (store != null) 
            {
                store.StoreName = storeDTO.StoreName;
                store.StoreLocation = storeDTO.StoreLocation;
                store.IsActive = storeDTO.IsActive;
                store.LogoKey = storeDTO.LogoKey;
            }
            await _storeRepository.UpdateStoreAsync(store);
        }
        public async Task AddStore(StoreDTO storeDTO, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var store = new Store
            {
                StoreName = storeDTO.StoreName,
                StoreLocation = storeDTO.StoreLocation,
                DateTimeRegistered = DateTime.Now,
                IsActive = storeDTO.IsActive,
                LogoKey = storeDTO.LogoKey,
                UserRegistered = user
            };
            await _storeRepository.AddStoreAsync(store);
        }

    }
}
