using Api.Models.User;
using Api.Services;
using AutoMapper;
using DAL.Entities;

namespace Api.Mapper.MapperActions
{
    public class UserAvatarMapperAction : IMappingAction<User, UserAvatarModel>
    {
        private LinkGeneratorService _links;
        public UserAvatarMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(User source, UserAvatarModel destination, ResolutionContext context) =>
            _links.FixAvatar(source, destination);

    }
}
