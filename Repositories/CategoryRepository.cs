using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CategoryDtos;
using blog_website_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_website_api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;

        public CategoryRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryDto>> GetAll()
        {
            var categories = await _context.Categories
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .ToListAsync();
            return categories;
        }
    }
}
