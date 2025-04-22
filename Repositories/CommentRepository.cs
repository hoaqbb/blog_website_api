using AutoMapper;
using AutoMapper.QueryableExtensions;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;
using blog_website_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace blog_website_api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(BlogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDto> CreateComment(CreateCommentDto createCommentDto, Guid userId)
        {
            var cmt = new PostComment
            {
                Content = createCommentDto.Content,
                UserId = userId,
                PostId = createCommentDto.PostId
            };

            await _context.AddAsync(cmt);

            if(await _context.SaveChangesAsync() > 0)
            {
                var createdCmt = await _context.PostComments
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.Id == cmt.Id);
                    
                return _mapper.Map<CommentDto>(createdCmt);
            }

            return null;
        }

        public async Task<PostComment?> GetCommentById(int id)
        {
            var cmt = await _context.PostComments.FindAsync(id);

            return cmt;
        }

        public async Task<List<CommentDto>> GetReplyComments(int commentId)
        {
            var replyCmts = await _context.PostComments
                .Where(x => x.ParentId == commentId)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return replyCmts;
        }

        public async Task<bool> IsCommentLikedByCurrentUser(Guid userId, int commentId)
        {
            var isLiked = await _context.CommentLikes
                .AnyAsync(x => x.UserId == userId && x.CommentId == commentId);
            return isLiked;
        }
    }
}
