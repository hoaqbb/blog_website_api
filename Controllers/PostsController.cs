using blog_website_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace blog_website_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ITokenService _tokenService;

        public PostsController(IPostRepository postRepository, ITokenService tokenService)
        {
            _postRepository = postRepository;
            _tokenService = tokenService;
        }

        [HttpGet("{slug}")]
        [HttpGet("{slug}", Name = "GetPost")]
        public async Task<ActionResult> GetPostBySlug(string slug)
        {
            var post = await _postRepository.GetPostBySlug(slug);
            if (post == null) return NotFound();

            //try to get user id from access token to check is current user liked this post
            HttpContext.Request.Cookies.TryGetValue("accessToken", out string? accessToken);
            if(accessToken != null)
            {
                var principal = _tokenService.GetPrincipalFromAccessToken(accessToken);
                var isUserLogedIn = Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);
                if (isUserLogedIn)
                    post.IsLikedByCurrentUser = await _postRepository.IsPostLikedByCurrentUser(userId, post.Id);
            }
            
            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            var isLoggedIn = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid authorId);
            if (!isLoggedIn) return Unauthorized();

            var post = await _postRepository.CreatePostAsync(createPostDto, authorId);

            if(post == null) return BadRequest("Problem creating post!");

            return CreatedAtRoute("GetPost", new {slug = post.Slug}, post);
        }
    }
}
