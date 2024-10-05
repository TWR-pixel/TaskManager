namespace TaskManager.PublicApi.Middlewares;

public sealed class HandleAllExceptionsMiddleware(ILogger<HandleAllExceptionsMiddleware> logger) : IMiddleware
{
    private readonly ILogger<HandleAllExceptionsMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);

            var jsonResponse = new
            {
                ExceptionMessage = ex.Message,
            };

            await context.Response.WriteAsJsonAsync(jsonResponse);
        }
    }
}
