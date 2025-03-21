using blog_website_api.Data.Entities;
using blog_website_api.DTOs.UserDtos;

namespace blog_website_api.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> ExternalLoginAsync(string email, string provider);
        Task<User> FindUserByEmailAsync(string email);
        Task<UserDto> AuthenticateAsync(LoginDto loginDto, HttpContext httpContext);
        Task<User> RegisterAsync(RegisterDto registerDto);
    }
}
