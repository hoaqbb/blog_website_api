using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class PostImage
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string PublicId { get; set; } = null!;
        public bool? IsThumbnail { get; set; }
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
