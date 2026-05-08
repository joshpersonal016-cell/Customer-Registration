using System.Text.Json;

namespace Customer.Registration.API.Middlewares
{
    public class ExceptionMiddleware (RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,

                Exception when ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    => StatusCodes.Status404NotFound,

                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                status = context.Response.StatusCode,
                message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
