
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

        public async Task<IEnumerable<PostListDto>> GetAll(Guid userId)
        {
            var posts = await _context.Posts
                .ProjectTo<PostListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var likedPostIds = await _context.PostLikes
                .Where(x => x.UserId == userId)
                .Select(x => x.PostId)
                .ToListAsync();

            foreach (var item in posts)
            {
                item.IsLikedByCurrentUser = likedPostIds.Contains(item.Id);
            }

            return posts;
        }

        public async Task<PaginatedResult<PostListDto>> GetPostsByCategory(
            PostSpecificationParams param, 
            ISpecification<Post> spec,
            Guid userId)
        {
            var inputQuery = _context.Set<Post>().AsQueryable();
            // query is paginated
            var query = SpecificationEvaluator<Post>
                .GetQuery<PostListDto>(inputQuery, spec, _mapper);
            // query is not paginated to count the number of item
            inputQuery = spec.ApplyCriteria(inputQuery);

            var totalItems = await inputQuery.CountAsync();
            var posts = await query.ToListAsync();

            // Get the list of postIds that the user has liked in the current page only
            var postIds = posts.Select(x => x.Id);
            var likedPostIds = await _context.PostLikes
                .Where(x => x.UserId == userId && postIds.Contains(x.PostId))
                .Select(x => x.PostId)
                .ToListAsync();

            // Reassign IsLikedByCurrentUser based on likedPostIds
            foreach ( var post in posts)
            {
                post.IsLikedByCurrentUser = likedPostIds.Contains(post.Id);
            }

            var result = new PaginatedResult<PostListDto>(posts, totalItems, param.PageIndex, param.PageSize);

            return result;
        }

        public async Task<Post> GetPostById(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);

            return post;
        }
    }
}
