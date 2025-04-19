using blog_website_api.DTOs.CategoryDtos;

namespace blog_website_api.DTOs.PostDtos
{
    public class PostListDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Slug { get; set; } = null!;
        public int? View { get; set; }
        public DateTime? CreateAt { get; set; }
        public AuthorDto Author { get; set; } = null!;
        public CategoryDto Category { get; set; }
        public string? Thumbnail { get; set; }
        public string Status { get; set; }
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public bool IsLikedByCurrentUser { get; set; }
    }
}
