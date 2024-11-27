using TriDViewAPI.DTO;

namespace TriDViewAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
    }
}
