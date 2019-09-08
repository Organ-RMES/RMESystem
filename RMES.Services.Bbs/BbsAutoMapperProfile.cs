using AutoMapper;
using RMES.Entity;

namespace RMES.Services.Bbs
{
    public class BbsAutoMapperProfile : Profile
    {
        public BbsAutoMapperProfile()
        {
            CreateMap<Topic, TopicListView>()
                .ForMember(target => target.Creator,
                    opt => opt.MapFrom(src => src.Creator == null ? "" : src.Creator.NickName))
                .ForMember(target => target.UpdateAt,
                    opt => opt.MapFrom(source => source.UpdateAt.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(target => target.LastCommentAt,
                    opt => opt.MapFrom(
                        source => source.LastCommentAt.HasValue ? source.LastCommentAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""));

            CreateMap<Post, PostView>()
                .ForMember(target => target.Author,
                    opt => opt.MapFrom(src => src.Creator == null ? "" : src.Creator.NickName))
                .ForMember(target => target.AuthorAvatar,
                    opt => opt.MapFrom(src => src.Creator == null ? "" : src.Creator.Avatar))
                .ForMember(target => target.AuthorGrade,
                    opt => opt.MapFrom(src => src.Creator == null ? 0 : src.Creator.Grade))
                .ForMember(target => target.UpdateAt,
                    opt => opt.MapFrom(source => source.UpdateAt.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Reply, ReplyView>()
                .ForMember(target => target.Creator,
                    opt => opt.MapFrom(src => src.Creator == null ? "" : src.Creator.NickName))
                .ForMember(target => target.CreateAt,
                    opt => opt.MapFrom(src => src.CreateAt.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(target => target.TargetUserName,
                    opt => opt.MapFrom(src => src.TargetUser == null ? "" : src.TargetUser.NickName));
        }
    }
}
