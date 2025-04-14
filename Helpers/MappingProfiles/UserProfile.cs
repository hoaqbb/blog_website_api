using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.PostDtos;
using blog_website_api.DTOs.UserDtos;

namespace blog_website_api.Helpers.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<User, UserDto>();
            CreateMap<User, AuthorDto>();
        }
    }
}
