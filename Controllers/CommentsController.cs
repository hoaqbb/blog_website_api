using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;
using blog_website_api.Interfaces;
using blog_website_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace blog_website_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITokenService _tokenService;

        public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository, ITokenService tokenService)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return BadRequest("Invalid client request!");

            var post = await _postRepository.GetPostById(createCommentDto.PostId);
            if (post == null) return NotFound("Post is not existed!");

            var cmt = await _commentRepository.CreateComment(createCommentDto, userId);

            if (cmt != null)
                return Ok(cmt);

            return BadRequest("Falied to comment post!");
        }

        [HttpGet("{commentId}/replies")]
        public async Task<ActionResult> GetReplyComments(int commentId)
        {
            HttpContext.Request.Cookies.TryGetValue("accessToken", out string? accessToken);
            var replyCmts = await _commentRepository.GetReplyComments(commentId);
            if (accessToken != null)
            {
                var principal = _tokenService.GetPrincipalFromAccessToken(accessToken);
                var isUserLogedIn = Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);
                if (isUserLogedIn)
                {
                    // Find and mark comments liked by current user
                    if (replyCmts.Any() == true)
                    {
                        await _postRepository.MarkCommentsLikedByUser(userId, replyCmts);
                    }
                }
            }

            if (replyCmts != null)
                return Ok(replyCmts);

            return BadRequest("Falied to load reply comments!");
        }
    }
}
