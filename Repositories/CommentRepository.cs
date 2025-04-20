using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;
using blog_website_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_website_api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<PostComment?> GetCommentById(int id)
        {
            var cmt = await _context.PostComments.FindAsync(id);

            return cmt;
        }

        public async Task<bool> IsCommentLikedByCurrentUser(Guid userId, int commentId)
        {
            var isLiked = await _context.CommentLikes
                .AnyAsync(x => x.UserId == userId && x.CommentId == commentId);

            return isLiked;
        }
    }
}
