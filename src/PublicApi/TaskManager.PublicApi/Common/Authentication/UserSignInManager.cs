using TaskManager.Application.Common.Security.Authentication;

namespace TaskManager.PublicApi.Common.Authentication;

public sealed class UserSignInManager : IUserSignInManager
{
    private readonly HttpContext _httpContext;

    public UserSignInManager(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public void Login(string refreshToken)
    {
        _httpContext.Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? tokenValue);

        if (tokenValue != refreshToken)
        {
            _httpContext.Response.Cookies.Delete(AuthConstants.REFRESH_TOKEN_COOKIE_NAME);

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true
            };

            _httpContext.Response.Cookies.Append(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, refreshToken, cookieOptions);
        }
    }

    public void Logout()
    {
        _httpContext.Response.Cookies.Delete(AuthConstants.REFRESH_TOKEN_COOKIE_NAME);
    }

    public void CreateRefreshToken(string refreshToken)
    {
        var options = new CookieOptions()
        {
            HttpOnly = true
        };

        _httpContext.Response.Cookies.Append(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, refreshToken, options);
    }
}
