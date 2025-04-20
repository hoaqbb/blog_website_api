using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;

namespace blog_website_api.Interfaces
{
    public interface ICommentRepository
    {
        Task<PostComment?> GetCommentById(int id);
        Task<bool> IsCommentLikedByCurrentUser(Guid userId, int commentId);
    }
}
