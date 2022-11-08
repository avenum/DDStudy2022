namespace Api.Models.Attach
{
    public class AttachModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string FilePath { get; set; } = null!;

    }

    public class AttachWithLinkModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string? ContentLink { get; set; } = null!;

        public AttachWithLinkModel(AttachModel model, Func<AttachModel, string?>? linkGenerator)
        {
            Id = model.Id;
            Name = model.Name;
            MimeType = model.MimeType;
            ContentLink = linkGenerator?.Invoke(model);
        }
    }
}
