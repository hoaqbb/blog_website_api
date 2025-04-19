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

        public LikesController(ILikeRepository likeRepository, IPostRepository postRepository)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
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

            return BadRequest("Falied to like post!");
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

            return BadRequest("Falied to unlike post!");
        }
    }
}
