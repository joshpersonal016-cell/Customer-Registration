namespace Customer.Registration.API.Middlewares
{
    public class ApiKeyMiddleware (RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string HEADER_NAME = "X-API-KEY";

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            if (!context.Request.Headers.TryGetValue(HEADER_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "API Key is missing!" });
                return;
            }

            var apiKey = config["ApiKey"];

            if (!apiKey!.Equals(extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid API Key!" });
                return;
            }

            await _next(context);
        }
    }
}
