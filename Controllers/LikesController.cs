using blog_website_api.Data.Entities;
using blog_website_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace blog_website_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public LikesController(ILikeRepository likeRepository, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpPost("post/{id}")]
        public async Task<ActionResult> LikePost(string id)
        {
            if (!Guid.TryParse(id, out Guid postId)
                || !Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId)) 
                return BadRequest("Invalid client request!");

            var post = await _postRepository.GetPostById(postId);
            if (post == null) return NotFound("Post is not existed!");

            if (await _postRepository.IsPostLikedByCurrentUser(userId, postId))
                return Conflict("Post is liked by current user!");

            if(await _likeRepository.LikePostAsync(postId, userId))
                return Ok();

            return BadRequest("Failed to like post!");
        }

        [Authorize]
        [HttpDelete("post/{id}")]
        public async Task<ActionResult> UnlikePost(string id)
        {
            if (!Guid.TryParse(id, out Guid postId)
               || !Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return BadRequest("Invalid client request!");

            var post = await _postRepository.GetPostById(postId);
            if (post == null) return NotFound("Post is not existed!");

            if (!await _postRepository.IsPostLikedByCurrentUser(userId, postId))
                return Conflict("Post is not liked by current user!");

            if (await _likeRepository.UnlikePostAsync(postId, userId))
                return Ok();

            return BadRequest("Failed to unlike post!");
        }

        [Authorize]
        [HttpPost("comment/{id}")]
        public async Task<ActionResult> LikeComment(int id)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return BadRequest("Invalid client request!");

            var cmt = await _commentRepository.GetCommentById(id);
            if (cmt == null) return NotFound("Comment is not existed!");

            if (await _commentRepository.IsCommentLikedByCurrentUser(userId, id))
                return Conflict("Comment is liked by current user!");

            if (await _likeRepository.LikeCommentAsync(id, userId))
                return Ok();

            return BadRequest("Failed to like comment!");
        }

        [Authorize]
        [HttpDelete("comment/{id}")]
        public async Task<ActionResult> UnlikeComment(int id)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
                return BadRequest("Invalid client request!");

            var cmt = await _commentRepository.GetCommentById(id);
            if (cmt == null) return NotFound("Comment is not existed!");

            if (!await _commentRepository.IsCommentLikedByCurrentUser(userId, id))
                return Conflict("Comment is not liked by current user!");

            if (await _likeRepository.UnlikeCommentAsync(id, userId))
                return Ok();

            return BadRequest("Failed to like comment!");
        }
    }
}
