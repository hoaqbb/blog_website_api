using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class User
    {
        public User()
        {
            CommentLikes = new HashSet<CommentLike>();
            PostComments = new HashSet<PostComment>();
            PostLikes = new HashSet<PostLike>();
            Posts = new HashSet<Post>();
        }

        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public byte[]? Password { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiryTime { get; set; }
        public string Provider { get; set; } = null!;
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;

        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
