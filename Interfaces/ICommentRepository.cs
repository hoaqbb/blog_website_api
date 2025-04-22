using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;

namespace blog_website_api.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentDto> CreateComment(CreateCommentDto createCommentDto, Guid userId);
        Task<PostComment?> GetCommentById(int id);
        Task<bool> IsCommentLikedByCurrentUser(Guid userId, int commentId);
        Task<List<CommentDto>> GetReplyComments(int commentId);
    }
}
