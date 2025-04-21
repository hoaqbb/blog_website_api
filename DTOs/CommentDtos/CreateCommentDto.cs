using System.ComponentModel.DataAnnotations;

namespace blog_website_api.DTOs.CommentDtos
{
    public class CreateCommentDto
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
