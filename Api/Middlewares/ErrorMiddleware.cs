using Api.Consts;
using Api.Exceptions;
using Api.Services;
using Common.Extentions;
using DAL.Entities;

namespace Api.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ex.Message);
                //await context.Response.CompleteAsync()
            }

            catch (HttpRequestException ex)
            {
                if (ex.StatusCode != null)
                    context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsJsonAsync(ex.Message);
            }


        }
    }
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorWrapper(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}
