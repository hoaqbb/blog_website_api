using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class Post
    {
        public Post()
        {
            PostComments = new HashSet<PostComment>();
            PostImages = new HashSet<PostImage>();
            PostLikes = new HashSet<PostLike>();
        }

        public string? Title { get; set; }
        public string Slug { get; set; } = null!;
        public string? Content { get; set; }
        public int? View { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Status { get; set; }
        public Guid AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public Guid Id { get; set; }
        public string? ShortDescription { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Category? Category { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
    }
}
