using Microsoft.Extensions.Options;
using System.Text;
using VetAuto.Models;

namespace VetAuto.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private const string PLAIN_TEXT_CONTENT_TYPE = "text/plain";

        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Error error = ex switch
                {
                    ArgumentException aex => new Error(400, aex.Message),
                    InvalidOperationException iex => new Error(400, iex.Message),
                    _ => new Error(500, ex.Message),
                };

                await WriteResponseAsync(httpContext, error);
            }
        }

        private async Task WriteResponseAsync(HttpContext httpContext, Error error)
        {
            httpContext.Response.StatusCode = error.StatusCode;
            httpContext.Response.Headers["Content-Type"] = PLAIN_TEXT_CONTENT_TYPE;
            await httpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(error.Message));
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static WebApplication UseErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }
    }
}
