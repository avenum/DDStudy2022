using System.Drawing;
using System.Xml.Linq;

namespace Api.Models.Attach
{
    public class MetadataModel
    {
        public Guid TempId { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long Size { get; set; }
    }

    public class MetadataLinkModel : MetadataModel
    {
        public string FilePath { get; set; } = null!;
        public Guid AuthorId { get; set; }
        //public MetaWithPath(MetadataModel model, Func<MetadataModel, string> pathgen, Guid authorId)
        //{
        //    TempId = model.TempId;
        //    Name = model.Name;
        //    MimeType = model.MimeType;
        //    Size = model.Size;
        //    FilePath = pathgen(model);
        //    AuthorId = authorId;
        //}
    }
}
