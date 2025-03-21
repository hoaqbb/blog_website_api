namespace blog_website_api.DTOs.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
