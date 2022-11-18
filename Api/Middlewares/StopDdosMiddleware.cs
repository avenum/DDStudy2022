using Api.Consts;
using Api.Exceptions;
using Api.Services;
using Common.Extentions;
using DAL.Entities;

namespace Api.Middlewares
{
    public class StopDdosMiddleware
    {
        private readonly RequestDelegate _next;

        public StopDdosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DdosGuard guard)
        {
            var headerAuth = context.Request.Headers.Authorization;

            try
            {
            guard.CheckRequest(headerAuth);
            await _next(context);

            }
            catch (TooManyRequestException)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync("too many Requests, allowed 10 request per second");

            }


        }
    }
    public static class StopDdosMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiDdosCustom(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StopDdosMiddleware>();
        }
    }
}
