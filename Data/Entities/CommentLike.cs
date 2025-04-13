using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class CommentLike
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int CommentId { get; set; }

        public virtual PostComment Comment { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
