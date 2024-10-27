using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAllStoreProductsAsync(int storeId);
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
