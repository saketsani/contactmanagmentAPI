using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace contactmanagmentAPI.Middleware
{
    public class Errorhandling
    {
        private readonly RequestDelegate _next;

        public Errorhandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                StatusCode = response.StatusCode,
                Message = exception.Message,
                Details = response.StatusCode == (int)HttpStatusCode.InternalServerError ? exception.StackTrace : null
            };

            var errorJson = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(errorJson);
        }
    }
}
