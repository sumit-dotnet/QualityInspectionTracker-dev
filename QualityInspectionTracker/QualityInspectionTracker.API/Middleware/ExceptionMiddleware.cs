using System.Text.Json;

namespace QualityInspectionTracker.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteError(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message);
            }
            catch (ArgumentException ex)
            {
                await WriteError(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteError(
                    context,
                    StatusCodes.Status409Conflict,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");

                await WriteError(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteError(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
