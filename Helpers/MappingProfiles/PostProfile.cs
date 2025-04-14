namespace blog_website_api.Helpers.MappingProfiles
{
    using AutoMapper;
    using blog_website_api.Data.Entities;
    using blog_website_api.Data.Enums;
    using blog_website_api.DTOs.PostDtos;

    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDetailDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                    src.Author))
                .ForMember(dest => dest.PostComments, opt => opt.MapFrom(src =>
                    src.PostComments.Where(x => x.ParentId == null)))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src =>
                    src.PostLikes.Count()))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                    src.PostComments.Count()))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore());
        }
    }
}
