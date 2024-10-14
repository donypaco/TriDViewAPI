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
        private readonly ILogService _logService;

        public StoreService(IStoreRepository storeRepository, IUserRepository userRepository, IConfiguration configuration, ILogService logService)
        {
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _logService = logService;
        }
        public async Task<Store> GetStoreById(int id)
        {
            try
            {
                return await _storeRepository.GetStoreByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logService.LogError("StoreService", ex.ToString());
                throw;
            }
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
                _logService.LogError("StoreService",ex.ToString());
                throw;
            }
            return Enumerable.Empty<StoreDTO>();
        }
        public async Task DeleteStore(int id)
        {
            try
            {
                await _storeRepository.DeleteStoreAsync(id);
            }
            catch (Exception ex)
            {
                _logService.LogError("StoreService", ex.ToString());
                throw;
            }
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
                    store.Description = storeDTO.Description;
                    store.StoreLocation = storeDTO.StoreLocation;
                    store.PlanID = storeDTO.PlanID;
                    store.IsActive = storeDTO.IsActive;
                    store.LogoKey = storeDTO.LogoKey;
                }
                await _storeRepository.UpdateStoreAsync(store);
            }
            catch (Exception ex) 
            {
                _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }
        public async Task AddStore(StoreDTO storeDTO, int userId)
        {
            try
            {
                var directoryPath = _configuration["directoryPath"];
                string fullPath = Path.Combine(directoryPath, storeDTO.LogoKey);
                byte[] imageByteArray = Convert.FromBase64String(storeDTO.Base64File);

                if (Directory.Exists(directoryPath))
                {
                    File.WriteAllBytes(fullPath, imageByteArray);
                }
                var user = await _userRepository.GetUserByIdAsync(userId);
                var store = new Store
                {
                    StoreName = storeDTO.StoreName,
                    Description = storeDTO.Description,   
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
                _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }

    }
}
