using TriDViewAPI.Data.Repositories.Interfaces;
using TriDViewAPI.DTO;
using TriDViewAPI.Services.Interfaces;

namespace TriDViewAPI.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogService _logService;
        public CategoryService(ICategoryRepository categoryRepository, ILogService logService)
        {
            _categoryRepository = categoryRepository;
            _logService = logService;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                return await _categoryRepository.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError("Category", ex.ToString());
            }
            return null;
        }
    }
}
