
using AutoMapper;
using AutoMapper.QueryableExtensions;
using blog_website_api.Data.Entities;
using blog_website_api.Data.Enums;
using blog_website_api.DTOs.BlogDtos;
using blog_website_api.DTOs.CommentDtos;
using blog_website_api.DTOs.PostDtos;
using blog_website_api.Helpers;
using blog_website_api.Helpers.Params;
using blog_website_api.Interfaces;
using blog_website_api.Specifications;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace blog_website_api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(BlogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PostDetailDto> GetPostBySlug(string slug)
        {
            var post = await _context.Posts
                .Where(p => p.Slug == slug && p.Status == 1)
                .ProjectTo<PostDetailDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (post == null) return null;

            var comments = await _context.PostComments
                .Where(c => c.ParentId == null && c.PostId == post.Id)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            post.PostComments = comments;

            return post;
        }

        public async Task<bool> IsPostLikedByCurrentUser(Guid userId, Guid postId)
        {
            var isLiked = await _context.PostLikes
                .AnyAsync(x => x.UserId == userId && x.PostId == postId);

            return isLiked;
        }
    }
}
