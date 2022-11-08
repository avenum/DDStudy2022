using Api.Models.Attach;
using DAL.Entities;

namespace Api.Models.Post
{
    public class CreatePostModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public Guid AuthorId { get; set; }
        public List<MetaWithPath> Contents { get; set; } = new List<MetaWithPath>();

    }
    public class CreatePostRequest
    {
        public string? Description { get; set; }
        public List<MetadataModel> Contents { get; set; } = new List<MetadataModel>();

    }
}
