using Api.Configs;
using Api.Models.Attach;
using Api.Models.Post;
using Api.Models.User;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly DAL.DataContext _context;
        private Func<Guid, string?>? _linkContentGenerator;
        private Func<Guid, string?>? _linkAvatarGenerator;
        public void SetLinkGenerator(Func<Guid, string?> linkContentGenerator, Func<Guid, string?> linkAvatarGenerator)
        {
            _linkAvatarGenerator = linkAvatarGenerator;
            _linkContentGenerator = linkContentGenerator;
        }
        public PostService(IMapper mapper, IOptions<AuthConfig> config, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreatePost(CreatePostRequest request)
        {
            var model = _mapper.Map<CreatePostModel>(request);

            model.Contents.ForEach(x =>
            {
                x.AuthorId = model.AuthorId;
                x.FilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "attaches",
                    x.TempId.ToString());

                var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), x.TempId.ToString()));
                if (tempFi.Exists)
                {
                    var destFi = new FileInfo(x.FilePath);
                    if (destFi.Directory != null && !destFi.Directory.Exists)
                        destFi.Directory.Create();

                    File.Move(tempFi.FullName, x.FilePath, true);
                }
            });




            var dbModel = _mapper.Map<Post>(model);
            await _context.Posts.AddAsync(dbModel);
            await _context.SaveChangesAsync();

        }

        public async Task<List<PostModel>> GetPosts(int skip, int take)
        {
            var posts = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostContents).AsNoTracking().OrderByDescending(x=>x.Created).Skip(skip).Take(take).ToListAsync();

            var res = posts.Select(post =>
                new PostModel
                {
                    Author = _mapper.Map<User, UserAvatarModel>(post.Author, o => o.AfterMap(FixAvatar)),
                    Description = post.Description,
                    Id = post.Id,
                    Contents = post.PostContents?.Select(x =>
                    _mapper.Map<PostContent, AttachExternalModel>(x, o => o.AfterMap(FixContent))).ToList()
                }).ToList();


            return res;

        }
        private void FixAvatar(User s, UserAvatarModel d)
        {
            d.AvatarLink = s.Avatar == null ? null : _linkAvatarGenerator?.Invoke(s.Id);
        }
        private void FixContent(PostContent s, AttachExternalModel d)
        {
            d.ContentLink = _linkContentGenerator?.Invoke(s.Id);
        }


        public async Task<AttachModel> GetPostContent(Guid postContentId)
        {
            var res = await _context.PostContents.FirstOrDefaultAsync(x => x.Id == postContentId);

            return _mapper.Map<AttachModel>(res);
        }
    }
}
