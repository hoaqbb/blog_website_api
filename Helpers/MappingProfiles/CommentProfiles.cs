using AutoMapper;
using blog_website_api.Data.Entities;
using blog_website_api.DTOs.CommentDtos;
using blog_website_api.DTOs.PostDtos;

namespace blog_website_api.Helpers.MappingProfiles
{
    public class CommentProfiles : Profile
    {
        public CommentProfiles()
        {
            CreateMap<PostComment, CommentDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                    src.User))
                .ForMember(dest => dest.ReplyCommentCount, opt => opt.MapFrom(src =>
                    src.InverseParent.Where(x => x.ParentId == src.Id).Count()))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src =>
                    src.CommentLikes.Count()))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore());
        }
    }
}
