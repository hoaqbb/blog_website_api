﻿using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class PostLike
    {
        public int Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
