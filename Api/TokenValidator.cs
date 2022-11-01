using Api.Services;
using DAL;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api
{
    public class TokenValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidatorMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context, UserService userService)
        {
            var isOk = true;
            var sessionIdString = context.User.Claims.FirstOrDefault(x => x.Type == "sessionId")?.Value;
            if (Guid.TryParse(sessionIdString, out var sessionId))
            {
                var session = await userService.GetSessionById(sessionId);
                if (!session.IsActive)
                {
                    isOk = false;
                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                   
                }
            }
            if (isOk)
            {
                await _next(context);
            }


            //var principal = new JwtSecurityTokenHandler().ValidateToken(, validParams, out var securityToken);




        }
    }
    public static class TokenValidatorMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidator(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidatorMiddleware>();
        }
    }
}
