using Api.Models.Attach;
using Api.Models.Post;
using Api.Models.User;
using AutoMapper;
using Common;

namespace Api
{
    public class MapperProfile: Profile
    {
        public MapperProfile() {
            CreateMap<CreateUserModel, DAL.Entities.User>()
                .ForMember(d=>d.Id, m=>m.MapFrom(s=>Guid.NewGuid()))
                .ForMember(d=>d.PasswordHash, m=>m.MapFrom(s=>HashHelper.GetHash(s.Password)))
                .ForMember(d=>d.BirthDay, m=>m.MapFrom(s=>s.BirthDate.UtcDateTime))
                ;
            CreateMap<DAL.Entities.User, UserModel>();

            CreateMap<DAL.Entities.Avatar, AttachModel>();
            
            CreateMap<DAL.Entities.PostContent, AttachModel>();

            CreateMap<MetadataModel, DAL.Entities.PostContent>();
            CreateMap<MetaWithPath, DAL.Entities.PostContent>();
            CreateMap<CreatePostModel, DAL.Entities.Post>()
                .ForMember(d => d.PostContents, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.Created, m => m.MapFrom(s => DateTime.UtcNow))

                ;



        }


    }
}
