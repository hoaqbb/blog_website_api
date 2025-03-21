using System.ComponentModel.DataAnnotations;

namespace blog_website_api.DTOs.UserDtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
