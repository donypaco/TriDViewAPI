using Microsoft.EntityFrameworkCore;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories
{
    public class StoreRepository: IStoreRepository
    {
        private readonly ApplicationDbContext _context;

        public StoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoreDTO>> GetAllActiveStoresAsync()
        {
            return await _context.Stores
                .Where(s => s.IsActive == true)
                .Select(s => new StoreDTO
                {
                    Id = s.Id,
                    StoreName = s.StoreName,
                    StoreLocation = s.StoreLocation,
                    LogoKey = s.LogoKey,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }
        public async Task<Store> GetStoreByIdAsync(int id)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task AddStoreAsync(Store store)
        {
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStoreAsync(Store store)
        {
            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteStoreAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
        }

    }
}
