namespace blog_website_api.Interfaces
{
    public interface ILikeRepository
    {
        Task<bool> LikePostAsync(Guid postId, Guid userId);
        Task<bool> UnlikePostAsync(Guid postId, Guid userId);
    }
}
