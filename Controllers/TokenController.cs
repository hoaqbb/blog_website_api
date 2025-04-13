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
        private readonly IAccountRepository _accountRepository;

        public TokenController(ITokenService tokenService, IAccountRepository accountRepository)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            HttpContext.Request.Cookies.TryGetValue("accessToken", out string accessToken);
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out string refreshToken);

            if (accessToken == null || refreshToken == null) return BadRequest("Invalid client request!");

            try
            {
                var principal = _tokenService.GetPrincipalFromExpinariedToken(accessToken);

                var isPrincipalContainUserId = Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);

                if (!isPrincipalContainUserId)
                {
                    _tokenService.RemoveTokenInsideCookies(HttpContext);
                    return Unauthorized();
                }

                var user = await _accountRepository.FindUserByIdAsync(userId);
                if (user == null
                    || user.RefreshToken != refreshToken
                    || user.TokenExpiryTime <= DateTime.UtcNow)
                {
                    //remove the cookie save token
                    _tokenService.RemoveTokenInsideCookies(HttpContext);
                    return BadRequest("Invalid client request!");
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                await _accountRepository.UpdateAsync(user);

                _tokenService.SetTokenInsideCookies(new TokenDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }, HttpContext);

                return Ok();
            }
            catch (Exception ex)
            {
                _tokenService.RemoveTokenInsideCookies(HttpContext);
                return BadRequest("Invalid token!");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("revoke")]
        public async Task<ActionResult> Revoke()
        {
            //neu access expinaried se ko dinh vao authen => 401
            //co the try get access token from http cookie de lay user id
            //ko lay tu User
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);

            var user = await _accountRepository.FindUserByIdAsync(userId);
            if (user == null) return BadRequest();

            await _accountRepository.RemoveUserTokenAsync(user);

            //remove refresh and access token cookie
            _tokenService.RemoveTokenInsideCookies(HttpContext);

            return NoContent();
        }
    }
}
