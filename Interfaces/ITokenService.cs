using blog_website_api.Data.Entities;
using blog_website_api.DTOs.UserDtos;
using Google.Apis.Auth;
using System.Security.Claims;

namespace blog_website_api.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        List<Claim> GenerateClaims(User user);
        ClaimsPrincipal GetPrincipalFromAccessToken(string token);
        void SetTokenInsideCookies(TokenDto tokenDto, HttpContext httpContext);
        void RemoveTokenInsideCookies(HttpContext httpContext);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string googleToken);
    }
}
