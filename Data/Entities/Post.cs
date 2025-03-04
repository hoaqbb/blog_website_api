using blog_website_api.Data.Enums;
using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Slug { get; set; } = null!;
        public string? Content { get; set; }
        public int? View { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Archive;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int AuthorId { get; set; }
        public int? CategoryId { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Category? Category { get; set; }
    }
}
