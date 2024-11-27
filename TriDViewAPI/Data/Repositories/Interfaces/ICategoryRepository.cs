using TriDViewAPI.DTO;

namespace TriDViewAPI.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    }
}
