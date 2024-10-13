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
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public StoreService(IStoreRepository storeRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<Store> GetStoreById(int id)
        {
            return await _storeRepository.GetStoreByIdAsync(id);
        }
        public async Task<IEnumerable<StoreDTO>> GetAllActiveStores()
        {
            try
            {
                var directoryPath = _configuration["directoryPath"];
                var stores = await _storeRepository.GetAllActiveStoresAsync();

                foreach (var store in stores)
                {
                    string fullPath = Path.Combine(directoryPath, store.LogoKey);
                    if (File.Exists(fullPath))
                    {
                        var imageByteArray = System.IO.File.ReadAllBytes(fullPath);
                        store.Base64File = Convert.ToBase64String(imageByteArray);
                    }
                }
                return stores;
            }
            catch (Exception ex) 
            {
            }
            return null;
        }
        public async Task DeleteStore(int id)
        {
            await _storeRepository.DeleteStoreAsync(id);
        }
        public async Task UpdateStore(StoreDTO storeDTO)
        {
            try
            {
                var store = await _storeRepository.GetStoreByIdAsync(storeDTO.Id);
                // mos harro planin
                if (store != null)
                {
                    store.StoreName = storeDTO.StoreName;
                    store.StoreLocation = storeDTO.StoreLocation;
                    store.IsActive = storeDTO.IsActive;
                    store.LogoKey = storeDTO.LogoKey;
                }
                await _storeRepository.UpdateStoreAsync(store);
            }
            catch (Exception ex) { }
        }
        public async Task AddStore(StoreDTO storeDTO, int userId)
        {
            try
            {
                var directoryPath = _configuration["directoryPath"];
                string fullPath = Path.Combine(directoryPath, storeDTO.LogoKey);
                byte[] imageByteArray = Convert.FromBase64String(storeDTO.Base64File);

                File.WriteAllBytes(fullPath, imageByteArray);

                var user = await _userRepository.GetUserByIdAsync(userId);
                var store = new Store
                {
                    StoreName = storeDTO.StoreName,
                    StoreLocation = storeDTO.StoreLocation,
                    DateTimeRegistered = DateTime.Now,
                    IsActive = storeDTO.IsActive,
                    PlanID = storeDTO.PlanID,
                    LogoKey = storeDTO.LogoKey,
                    UserRegistered = user
                };
                await _storeRepository.AddStoreAsync(store);
            }
            catch(Exception ex)
            {

            }
        }

    }
}
