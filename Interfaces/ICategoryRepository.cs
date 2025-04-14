using blog_website_api.DTOs.CategoryDtos;

namespace blog_website_api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDto>> GetAll();
    }
}
