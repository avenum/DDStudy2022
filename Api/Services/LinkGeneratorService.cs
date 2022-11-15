using Api.Models.Attach;
using Api.Models.User;
using DAL.Entities;

namespace Api.Services
{
    public class LinkGeneratorService
    {
        public Func<PostContent, string?>? LinkContentGenerator;
        public Func<User, string?>? LinkAvatarGenerator;

        public void FixAvatar(User s, UserAvatarModel d)
        {
            d.AvatarLink = s.Avatar == null ? null : LinkAvatarGenerator?.Invoke(s);
        }
        public void FixContent(PostContent s, AttachExternalModel d)
        {
            d.ContentLink = LinkContentGenerator?.Invoke(s);
        }

    }
}
