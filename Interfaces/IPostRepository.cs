using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.BlogDtos;
using blog_website_api.DTOs.PostDtos;
using blog_website_api.Helpers;
using blog_website_api.Helpers.Params;

namespace blog_website_api.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostListDto>> GetAll(Guid userId);
        Task<PostDetailDto> GetPostBySlug(string slug);
        Task<Post> GetPostById(Guid id);
        Task<bool> IsPostLikedByCurrentUser(Guid userId, Guid postId);
        Task<PostDetailDto> CreatePostAsync(CreatePostDto createPostDto, Guid authorId);
        Task<PaginatedResult<PostListDto>> GetPostsByCategory(
                PostSpecificationParams postSpecificationParams,
                ISpecification<Post> spec,
                Guid userId
            );
    }
}
