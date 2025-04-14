using blog_website_api.Data.Entities;
using blog_website_api.Data.Enums;
using blog_website_api.DTOs.PostDtos;
using System.ComponentModel.DataAnnotations;

namespace blog_website_api.DTOs.BlogDtos
{
    public class CreatePostDto
    {
        [Required]
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public int CategoryId { get; set; }
        public int Status { get; set; }
        public List<PostImageDto>? PostImages { get; set; }
    }
}
