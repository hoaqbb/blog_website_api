using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
