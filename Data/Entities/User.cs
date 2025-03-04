using System;
using System.Collections.Generic;

namespace blog_website_api.Data.Entities
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiryTime { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
