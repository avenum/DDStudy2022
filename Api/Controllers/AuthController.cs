using Api.Models.Token;
using Api.Models.User;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName ="Auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        public AuthController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<TokenModel> Token(TokenRequestModel model)
        {
            try
            {
            var token = await _authService.GetToken(model.Login, model.Pass);

            return token;

            }
            catch (Exception)
            {
                throw new HttpRequestException("not authorize", null ,statusCode: System.Net.HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
            => await _authService.GetTokenByRefreshToken(model.RefreshToken);

        [HttpPost]
        public async Task RegisterUser(CreateUserModel model)
        {
            if (await _userService.CheckUserExist(model.Email))
                throw new Exception("user is exist");
            await _userService.CreateUser(model);

        }

    }
}
