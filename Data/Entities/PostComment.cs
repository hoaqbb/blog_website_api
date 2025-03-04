using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class PostComment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int PostId { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
