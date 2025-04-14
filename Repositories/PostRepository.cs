
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

        public async Task<PostDetailDto> CreatePostAsync(CreatePostDto createPostDto, Guid authorId)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                ShortDescription = createPostDto.ShortDescription,
                Slug = GenerateSlug(createPostDto.Title),
                AuthorId = authorId,
                CategoryId = createPostDto.CategoryId,
                Status = createPostDto.Status
            };
            await _context.AddAsync(post);

            var postImages = new List<PostImage>();
            if (createPostDto.PostImages != null)
            {
                foreach (var img in createPostDto.PostImages)
                {
                    var postImg = new PostImage
                    {
                        PostId = post.Id,
                        PublicId = img.PublicId,
                        Url = img.ImageUrl,
                        IsThumbnail = img.IsThumbnail
                    };
                    postImages.Add(postImg);
                }
                await _context.AddRangeAsync(postImages);
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<PostDetailDto>(post);
        }

        //generate slug with slugify lib
        private string GenerateSlug(string title)
        {
            var randomNumber = new Random().Next(10000000);
            return new SlugHelper()
                .GenerateSlug(title + " " + randomNumber.ToString());
        }
    }
}
