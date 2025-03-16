
using Microsoft.Extensions.Options;
using TriDViewAPI.Configurations;
using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Helpers;
using TriDViewAPI.Models;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        //private readonly ICategoryRepository _categoryRepository;
        private readonly ILogService _logService;
        //private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;
        public ProductService( IProductRepository productRepository, IUserRepository userRepository, 
           ILogService logService, IOptions<AppSettings> options)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            //_categoryRepository = categoryRepository;
            _logService = logService;
            //_configuration = configuration;
            _appSettings = options.Value;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllStoreProducts(int storeId)
        {
            try
            {
                var directoryPath = _appSettings.LogoDirectoryPath;

                IEnumerable<ProductDTO> products = await _productRepository.GetAllStoreProductsAsync(storeId);
                foreach (var product in products)
                {
                    product.Base64File = await HelperMethods.FindImage(directoryPath, product.ImageUrl);
                }
                return products;
            }
            catch (Exception ex)
            {
                _logService.LogError("ProductService", ex.ToString());
            }
            return Enumerable.Empty<ProductDTO>();
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                var directoryPath = _appSettings.LogoDirectoryPath;
                var product = await _productRepository.GetProductByIdAsync(id);

                ProductDTO productDTO = new ProductDTO
                {
                    Name = product.Name,
                    CategoryID = product.CategoryID,
                    Description = product.Description,
                    Price = product.Price,
                    Rating = product.Rating,
                    Height = product.Height,
                    Weight = product.Weight,
                    IsActive = product.IsActive,
                    Quantity = product.Quantity
                };

                productDTO.Base64File = await HelperMethods.FindImage(directoryPath, productDTO.ImageUrl);
                return productDTO;
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
        public async Task UpdateProduct(ProductDTO productDTO)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(productDTO.ProductID);
                if (product != null)
                {
                    await _productRepository.UpdateProductAsync(product);
                }
            }
            catch (Exception ex)
            {
                await _logService.LogError("ProductService", ex.ToString());
                throw;
            }
        }
        public async Task AddProduct(ProductDTO productDTO, int userId, IFormFile image)
        {
            try
            {
                string directoryPath = _appSettings.ProductsDirectoryPath;
                string fileName = image?.FileName ?? "default-product.png";

                fileName = await HelperMethods.SavePhotoToPathAsync(directoryPath, fileName, image);

                var user = await _userRepository.GetUserByIdAsync(userId);
                var product = new Product
                {
                    Name = productDTO.Name,
                    DateAdded = DateTime.Now,
                    Description = productDTO.Description,
                    Height = productDTO.Height,
                    Weight = productDTO.Weight,
                    SKU = productDTO.SKU,
                    Discount = productDTO.Discount,
                    ImageUrl = fileName,
                    IsActive = productDTO.IsActive.GetValueOrDefault(),
                    Price = productDTO.Price,
                    Tags = productDTO.Tags,
                    CategoryID = productDTO.CategoryID,
                    UserID = userId
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
