using System.Xml.Linq;

namespace Api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTimeOffset BirthDate { get; set; }
        public int PostsCount { get; set; }
    }

    public class UserAvatarModel : UserModel
    {
        public string? AvatarLink { get; set; }
        //public UserAvatarModel(UserModel model, Func<UserModel, string?>? linkGenerator)
        //{
        //    Id = model.Id;
        //    Name = model.Name;
        //    Email = model.Email;
        //    BirthDate = model.BirthDate;
        //    AvatarLink = linkGenerator?.Invoke(model);
        //}
    }
}
