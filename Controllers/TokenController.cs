using blog_website_api.Data.Entities;
using blog_website_api.DTOs.UserDtos;
using blog_website_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace blog_website_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly BlogDbContext _context;

        public TokenController(ITokenService tokenService, BlogDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            HttpContext.Request.Cookies.TryGetValue("accessToken", out string accessToken);
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out string refreshToken);

            if (accessToken == null || refreshToken == null) return BadRequest("Invalid client request!");

            var principal = _tokenService.GetPrincipalFromExpinariedToken(accessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null) return Unauthorized();

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null
                || user.RefreshToken != refreshToken
                || user.TokenExpiryTime <= DateTime.UtcNow)
                return BadRequest("Invalid client request!");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            _tokenService.SetTokenInsideCookies(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }, HttpContext);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("revoke")]
        public async Task<ActionResult> Revoke()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _context.Users.SingleOrDefault(u => u.Id.ToString() == userId);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            user.TokenExpiryTime = null;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
