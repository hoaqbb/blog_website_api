namespace blog_website_api.DTOs.PostDtos
{
    public class AuthorDto
    {
        public string Id { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string? Avatar { get; set; }
    }
}
