using Api.Consts;
using Api.Models.Attach;
using Api.Models.Post;
using Api.Services;
using Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
            _postService.SetLinkGenerator(
                linkAvatarGenerator: x =>
                Url.Action(nameof(UserController.GetUserAvatar), "User", new
                {
                    userId = x.Id,
                    download = false
                }),
                linkContentGenerator: x => Url.Action(nameof(GetPostContent), new
                {
                    postContentId = x.Id,
                    download = false
                }))
                 ;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<FileStreamResult> GetPostContent(Guid postContentId, bool download = false)
        {
            var attach = await _postService.GetPostContent(postContentId);
            var fs = new FileStream(attach.FilePath, FileMode.Open);
            if (download)
                return File(fs, attach.MimeType, attach.Name);
            else
                return File(fs, attach.MimeType);

        }

        [HttpGet]
        public async Task<List<PostModel>> GetPosts(int skip = 0, int take = 10) 
            => await _postService.GetPosts(skip, take);

        [HttpPost]
        public async Task CreatePost(CreatePostRequest request)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new Exception("not authorize");

            var model = new CreatePostModel
            {
                AuthorId = userId,
                Description = request.Description,
                Contents = request.Contents.Select(x =>
                new MetaWithPath(x, q => Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "attaches",
                    q.TempId.ToString()), userId)).ToList()
            };

            model.Contents.ForEach(x =>
            {
                var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), x.TempId.ToString()));
                if (tempFi.Exists)
                {
                    var destFi = new FileInfo(x.FilePath);
                    if (destFi.Directory != null && !destFi.Directory.Exists)
                        destFi.Directory.Create();

                    System.IO.File.Copy(tempFi.FullName, x.FilePath, true);
                    tempFi.Delete();
                }

            });

            await _postService.CreatePost(model);

        }

    }
}
