using Moq;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services;

namespace TriDViewTests
{
    public class StoreServiceTests
    {
        private readonly Mock<IStoreRepository> _storeRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly StoreService _storeService;

        public StoreServiceTests()
        {
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _storeService = new StoreService(_storeRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllActiveStores_ShouldReturnActiveStores()
        {
            var stores = new List<StoreDTO>
            {
                new StoreDTO { Id = 1, StoreName = "Store 1", IsActive = true },
                new StoreDTO { Id = 2, StoreName = "Store 2", IsActive = false }
            };

            _storeRepositoryMock.Setup(repo => repo.GetAllActiveStoresAsync())
                .ReturnsAsync(stores.Where(s => s.IsActive));

            var result = await _storeService.GetAllActiveStores();

            Assert.Single(result);
            Assert.Equal("Store 1", result.First().StoreName);
        }

        [Fact]
        public async Task GetStoreById_ShouldReturnStore()
        {
            int storeId = 1;
            var store = new Store { Id = storeId, StoreName = "Store 1", IsActive = true };

            _storeRepositoryMock.Setup(repo => repo.GetStoreByIdAsync(storeId))
                .ReturnsAsync(store);

            var result = await _storeService.GetStoreById(storeId);

            Assert.Equal(storeId, result.Id);
            Assert.Equal(store.StoreName, result.StoreName);
        }
        [Fact]
        public async Task DeleteStoreById_ShouldDeleteStore()
        {
            int storeId = 1;

            _storeRepositoryMock.Setup(repo => repo.DeleteStoreAsync(storeId))
                .Returns(Task.CompletedTask);

            await _storeService.DeleteStore(storeId);

            _storeRepositoryMock.Verify(repo => repo.DeleteStoreAsync(storeId), Times.Once);
        }
        [Fact]
        public async Task UpdateStoreById_ShouldUpdateStore()
        {
            var store = new Store { Id = 1, StoreName = "Old Store", IsActive = false };
            var storeDTO = new StoreDTO { Id = 1, StoreName = "New Updated Store", IsActive = true, LogoKey = "new-logo-key" };

            _storeRepositoryMock.Setup(repo => repo.GetStoreByIdAsync(store.Id))
                .ReturnsAsync(store);

            await _storeService.UpdateStore(storeDTO);

            Assert.Equal("New Updated Store", store.StoreName);
            Assert.Equal(true, store.IsActive);
            Assert.Equal("new-logo-key", store.LogoKey);
            _storeRepositoryMock.Verify(repo => repo.UpdateStoreAsync(store), Times.Once);
        }
        [Fact]
        public async Task AddStore_ShouldCreateStore()
        {
            int userId = 2;
            var storeDTO = new StoreDTO { Id = 1, StoreName = "New Updated Store", IsActive = true, LogoKey = "new-logo-key" };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(new User());

            _storeRepositoryMock.Setup(repo => repo.AddStoreAsync(It.IsAny<Store>()))
                .Returns(Task.CompletedTask);

            await _storeService.AddStore(storeDTO, userId);

            _storeRepositoryMock.Verify(repo => repo.AddStoreAsync(It.IsAny<Store>()), Times.Once);
        }

    }
}