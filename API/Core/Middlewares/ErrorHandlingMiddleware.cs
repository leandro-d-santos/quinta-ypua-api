using Configuration.Configuration;
using Configuration.Models;
using System.Net;
using System.Text.Json;

namespace Configuration.Middlewares
{
    public sealed class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly IAppSettings _appSettings;

        public ErrorHandlingMiddleware(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleError(context, ex);
            }
        }

        private async Task HandleError(HttpContext context, Exception exception)
        {
            ExceptionResponse response = new();
            if (_appSettings.IsDebug)
            {
                response.Title = "Error";
                response.Message = $"{exception.Message}";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                response.Title = "Error";
                response.Message = "Internal server error";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}