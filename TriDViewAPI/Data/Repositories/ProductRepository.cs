using Microsoft.EntityFrameworkCore;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllStoreProductsAsync(int storeId)
        {
            return await _context.Products
                .Where(p => p.StoreID == storeId)
                .Select(p => new ProductDTO
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Category = p.Category.CategoryName,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Height = p.Height,
                    Width = p.Width,
                    SKU = p.SKU,
                    Weight = p.Weight
                })
                .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == id);
        }
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}
