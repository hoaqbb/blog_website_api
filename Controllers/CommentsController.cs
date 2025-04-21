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

        public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
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
    }
}
