namespace blog_website_api.DTOs.PostDtos
{
    public class PostImageDto
    {
        public string PublicId { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsThumbnail { get; set; }
    }
}
