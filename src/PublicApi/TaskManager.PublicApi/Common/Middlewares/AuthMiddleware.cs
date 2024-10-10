
namespace TaskManager.PublicApi.Common.Middlewares;

public sealed class AuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //context.Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out var refreshToken);

        await next(context);
    }
}
