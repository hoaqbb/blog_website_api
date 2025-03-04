using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
