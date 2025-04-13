using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class PostComment
    {
        public PostComment()
        {
            CommentLikes = new HashSet<CommentLike>();
            InverseParent = new HashSet<PostComment>();
        }

        public int Id { get; set; }
        public string? Content { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateAt { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public virtual PostComment? Parent { get; set; }
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<PostComment> InverseParent { get; set; }
    }
}
