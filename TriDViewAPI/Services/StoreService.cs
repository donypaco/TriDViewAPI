using Microsoft.AspNetCore.Identity;
using TriDViewAPI.Data;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;
using TriDViewAPI.Helpers;

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
                await _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }
        public async Task<IEnumerable<StoreDTO>> GetAllActiveStores()
        {
            try
            {
                var directoryPath = _configuration["LogoDirectoryPath"];
                var stores = await _storeRepository.GetAllActiveStoresAsync();
                //var storeDTOs = new List<StoreDTO>();

                foreach (var store in stores)
                {
                    //var storeDto = new StoreDTO
                    //{
                    //    Id = store.Id,
                    //    Description = store.Description,
                    //    StoreName = store.StoreName,
                    //    StoreLocation = store.StoreLocation,
                    //    PlanID = store.PlanID,
                    //    IsActive = store.IsActive,
                    //    LogoKey = store.LogoKey
                    //};

                    store.Base64File = await HelperMethods.FindImage(directoryPath, store.LogoKey);
                    
                    //storeDTOs.Add(store);
                }

                return stores;
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreService",ex.ToString());
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
                var store = await _storeRepository.GetStoreByIdAsync(storeDTO.Id.GetValueOrDefault());
                // mos harro planin
                if (store != null)
                {
                    store.StoreName = storeDTO.StoreName;
                    store.Description = storeDTO.Description;
                    store.StoreLocation = storeDTO.StoreLocation;
                    store.PlanID = storeDTO.PlanID;
                    store.IsActive = storeDTO.IsActive.GetValueOrDefault();
                    store.LogoKey = storeDTO.LogoKey;
                }
                await _storeRepository.UpdateStoreAsync(store);
            }
            catch (Exception ex) 
            {
                await _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }
        public async Task AddStore(StoreDTO storeDTO, int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                var store = new Store
                {
                    StoreName = storeDTO.StoreName,
                    Description = storeDTO.Description,   
                    StoreLocation = storeDTO.StoreLocation,
                    DateTimeRegistered = DateTime.Now,
                    IsActive = storeDTO.IsActive.GetValueOrDefault(),
                    PlanID = storeDTO.PlanID,
                    LogoKey = storeDTO.LogoKey,
                    UserRegistered = user
                };
                await _storeRepository.AddStoreAsync(store);
            }
            catch(Exception ex)
            {
                await _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }
        public async Task RegisterStore(StoreDTO storeDTO, IFormFile logo)
        {
            try
            {
                string directoryPath = _configuration["LogoDirectoryPath"];
                string logoFileName = logo?.FileName ?? "default-logo.png";

                logoFileName = await HelperMethods.SavePhotoToPathAsync(directoryPath, logoFileName, logo);

                var store = new Store
                {
                    StoreName = storeDTO.StoreName,
                    Description = storeDTO.Description,
                    StoreLocation = storeDTO.StoreLocation,
                    DateTimeRegistered = DateTime.Now,
                    IsActive = storeDTO.IsActive.GetValueOrDefault(),
                    PlanID = storeDTO.PlanID,
                    LogoKey = logoFileName
                };
                await _storeRepository.AddStoreAsync(store);

            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreService", $"Error registering store '{storeDTO.StoreName}': {ex}");
                throw;
            }
        }
        public async Task ConfirmRegistration(int storeId)
        {
            try
            {
                var store = await _storeRepository.GetStoreByIdAsync(storeId);
                if (store != null)
                {
                    store.IsActive = true;
                    await _storeRepository.UpdateStoreAsync(store);
                }
            }
            catch (Exception ex)
            {
                await _logService.LogError("StoreService", ex.ToString());
                throw;
            }
        }

    }
}
