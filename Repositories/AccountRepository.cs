using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.UserDtos;
using blog_website_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace blog_website_api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountRepository(BlogDbContext context, IMapper mapper, ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserDto> AuthenticateAsync(LoginDto loginDto, HttpContext httpContext)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == loginDto.Email && x.Provider == "Local");
            if (user is null) return null;

            var passwordHash = HashPassword(loginDto.Password);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.Password[i]) return null;
            }

            var claims = _tokenService.GenerateClaims(user);

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.TokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.Update(user);
            await _context.SaveChangesAsync();

            _tokenService.SetTokenInsideCookies(new TokenDto 
            { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            }, httpContext);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<User> ExternalLoginAsync(string email, string provider)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Provider == provider);

            return user;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            //var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            user.Id = Guid.NewGuid();
            user.Password = HashPassword(registerDto.Password);
            user.Provider = "Local";

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task RemoveUserTokenAsync(User user)
        {
            user.RefreshToken = null;
            user.TokenExpiryTime = null;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private byte[] HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var passwordHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return passwordHash;
        }
    }
}
