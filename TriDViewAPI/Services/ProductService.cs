
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogService _logService;
        public ProductService( IProductRepository productRepository, IConfiguration configuration, ILogService logService)
        {
            _productRepository = productRepository;
            _logService = logService;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllStoreProducts(int storeId)
        {
            try
            {
                return await _productRepository.GetAllStoreProductsAsync(storeId);
            }
            catch (Exception ex)
            {
                _logService.LogError("ProductService", ex.ToString());
            }
            return Enumerable.Empty<ProductDTO>();
            ;
        }
        public async Task<Product> GetProductById(int id)
        {
            try
            {
                return await _productRepository.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductService", ex.ToString());
                throw;
            }
        }
        public async Task DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                _logService.LogError("ProductService", ex.ToString());
                throw;
            }
        }
        public async Task UpdateProduct(ProductDTO ProductDTO)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(productDTO.ProductId.GetValueOrDefault());
                if (product != null)
                {
                }
                await _productRepository.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductService", ex.ToString());
                throw;
            }
        }
        public async Task AddProduct(ProductDTO ProductDTO, int userId)
        {
            try
            {
                //var directoryPath = _configuration["directoryPath"];
                //if (storeDTO.LogoKey != null)
                //{
                //    string fullPath = Path.Combine(directoryPath, storeDTO.LogoKey);
                //    byte[] imageByteArray = Convert.FromBase64String(storeDTO.Base64File);

                //    if (Directory.Exists(directoryPath))
                //    {
                //        File.WriteAllBytes(fullPath, imageByteArray);
                //    }
                //}

                var user = await _userRepository.GetUserByIdAsync(userId);
                var product = new Product
                {
                };
                await _productRepository.AddProductAsync(product);
            }
            catch(Exception ex)
            {
                await _logService.LogError("ProductService", ex.ToString());
                throw;
            }
        }

    }
}
