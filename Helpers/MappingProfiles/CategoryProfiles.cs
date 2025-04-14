using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CategoryDtos;

namespace blog_website_api.Helpers.MappingProfiles
{
    public class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
