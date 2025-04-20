using blog_website_api.Data.Entities;
using blog_website_api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_website_api.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly BlogDbContext _context;

        public LikeRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LikePostAsync(Guid postId, Guid userId)
        {
            var newLike = new PostLike
            {
                PostId = postId,
                UserId = userId
            };
            await _context.AddAsync(newLike);
            return (await _context.SaveChangesAsync() > 0) ? true : false;
        }

        public async Task<bool> UnlikePostAsync(Guid postId, Guid userId)
        {
            var isLiked = await _context.PostLikes
                .SingleOrDefaultAsync(x => x.PostId == postId && x.UserId == userId);

            if (isLiked == null) return false;
            _context.Remove(isLiked);
            return (await _context.SaveChangesAsync() > 0) ? true : false;
        }
        public async Task<bool> LikeCommentAsync(int commentId, Guid userId)
        {
            var newLike = new CommentLike
            {
                CommentId = commentId,
                UserId = userId
            };
            await _context.AddAsync(newLike);
            return (await _context.SaveChangesAsync() > 0) ? true : false;
        }

        public async Task<bool> UnlikeCommentAsync(int commentId, Guid userId)
        {
            var isLiked = await _context.CommentLikes
                .SingleOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId);

            if (isLiked == null) return false;
            _context.Remove(isLiked);
            return (await _context.SaveChangesAsync() > 0) ? true : false;
        }
    }
}
