using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;

namespace blog_website_api.DTOs.PostDtos
{
    public class PostDetailDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string Slug { get; set; } = null!;
        public int? View { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public AuthorDto Author { get; set; } = null!;
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public bool IsLikedByCurrentUser { get; set; }
        //public virtual Category? Category { get; set; }
        //public List<PostImage> PostImages { get; set; }
        public List<CommentDto>? PostComments { get; set; }
    }
}
