using blog_website_api.Data.Entities;
using blog_website_api.DTOs.BlogDtos;
using blog_website_api.Helpers.Params;
using blog_website_api.Interfaces;
using blog_website_api.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize]
        [HttpGet("category")]
        public async Task<ActionResult> GetWithCat([FromQuery] PostSpecificationParams postSpecificationParams)
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);
            var spec = new PostSpecification(postSpecificationParams, userId);

            var result = await _postRepository.GetPostsByCategory(postSpecificationParams, spec, userId);

            return Ok(result);
        }

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
