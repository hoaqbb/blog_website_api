using System.ComponentModel.DataAnnotations;

namespace blog_website_api.DTOs.UserDtos
{
    public class ExternalAuthDto
    {
        //[Required]
        //public string Provider { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
