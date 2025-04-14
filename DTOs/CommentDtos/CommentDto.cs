using blog_website_api.DTOs.PostDtos;
using System.Text.Json.Serialization;

namespace blog_website_api.DTOs.CommentDtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public AuthorDto Author { get; set; } = null!;
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ReplyCommentCount { get; set; } = 0;
    }
}
