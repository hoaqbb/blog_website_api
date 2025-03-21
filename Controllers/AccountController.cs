using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.UserDtos;
using blog_website_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Principal;

namespace blog_website_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountRepository _accountRepository;
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;

        public AccountController(
            ITokenService tokenService, 
            IAccountRepository accountRepository, 
            BlogDbContext context,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _accountRepository.AuthenticateAsync(loginDto, HttpContext);
            if (user is null) return BadRequest("Invalid email or password!");

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = await _accountRepository.FindUserByEmailAsync(registerDto.Email);
            if (user != null && user.Provider == "Local") 
            {
                return BadRequest("User is existed!");            
            }

            try
            {
                user = await _accountRepository.RegisterAsync(registerDto);
                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.TokenExpiryTime = DateTime.UtcNow.AddDays(7);

                _context.Users.Update(user);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var claims = _tokenService.GenerateClaims(user);
                    var accessToken = _tokenService.GenerateAccessToken(claims);

                    _tokenService.SetTokenInsideCookies(new TokenDto 
                    { 
                        AccessToken = accessToken, 
                        RefreshToken = user.RefreshToken 
                    }, 
                        HttpContext);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost("login-with-google")]
        public async Task<ActionResult<UserDto>> LoginWithGoogle([FromBody]ExternalAuthDto externalAuthDto)
        {
            var payload = await _tokenService.VerifyGoogleToken(externalAuthDto.Token);
            if (payload == null)
            {
                return BadRequest("Invalid External Authentication.");
            }

            // Kiểm tra user đã tồn tại chưa
            var user = await _accountRepository.ExternalLoginAsync(payload.Email, payload.Issuer)
                       ?? await _accountRepository.FindUserByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    FullName = payload.Name,
                    Provider = payload.Issuer,
                    Avatar = payload.Picture,
                    RefreshToken = _tokenService.GenerateRefreshToken(),
                    TokenExpiryTime = DateTime.UtcNow.AddDays(7)
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Cập nhật refresh token cho user
                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.TokenExpiryTime = DateTime.UtcNow.AddDays(7);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            // Tạo access token và lưu vào cookie
            var claims = _tokenService.GenerateClaims(user);

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = user.RefreshToken;

            _tokenService.SetTokenInsideCookies(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }, HttpContext);

            return Ok(_mapper.Map<UserDto>(user));
        }


    }
}
