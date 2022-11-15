using Api.Models.Attach;
using Api.Services;
using AutoMapper;
using DAL.Entities;

namespace Api.Mapper.MapperActions
{
    public class PostContentMapperAction : IMappingAction<PostContent, AttachExternalModel>
    {
        private LinkGeneratorService _links;
        public PostContentMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(PostContent source, AttachExternalModel destination, ResolutionContext context)
            => _links.FixContent(source, destination);
    }
}
