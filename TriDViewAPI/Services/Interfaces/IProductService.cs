using TriDViewAPI.DTO;
using TriDViewAPI.Models;

namespace TriDViewAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductById(int id);
        Task<IEnumerable<ProductDTO>> GetAllStoreProducts(int storeId);
        Task DeleteProduct(int id);
        Task UpdateProduct(ProductDTO product);
        Task AddProduct(ProductDTO productDTO, int userId, IFormFile image);
    }
}
