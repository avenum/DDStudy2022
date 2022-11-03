namespace Api.Models
{
    public class AddAvatarRequestModel
    {
        public MetadataModel Avatar { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
