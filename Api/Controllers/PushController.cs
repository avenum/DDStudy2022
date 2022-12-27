using Api.Consts;
using Api.Models.Push;
using Api.Services;
using Common.Extentions;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PushController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly GooglePushService _googlePushService;
        public PushController(UserService userService, GooglePushService googlePushService)
        {
            _userService = userService;
            _googlePushService = googlePushService;
        }

        [HttpPost]
        public async Task Subscribe(PushTokenModel model)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId != default)
            {
                await _userService.SetPushToken(userId, model.Token);

            }
        }
        [HttpDelete]
        public async Task Unsubscribe()
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId != default)
            {

                await _userService.SetPushToken(userId);
            }
        }

        [HttpPost]
        public async Task<List<string>> SendPush(SendPushModel model)
        {
            var res = new List<string>();
            var userId = model.UserId ?? User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId != default)
            {
                var token = await _userService.getPushToken(userId);
                if (token != default)
                {
                    res = _googlePushService.SendNotification(token, model.Push);
                }
                //TODO 
            }

            return res;
        }
    }
}
