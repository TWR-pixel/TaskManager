namespace TaskManager.PublicApi.Common.Authentication;

public sealed class UserSignInManager : IUserSignInManager
{

    public void Login(string refreshToken, HttpContext context)
    {
        context.Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? tokenValue);

        if (tokenValue != refreshToken)
        {
            context.Response.Cookies.Delete(AuthConstants.REFRESH_TOKEN_COOKIE_NAME);

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true
            };

            context.Response.Cookies.Append(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, refreshToken, cookieOptions);
        }
    }

    public void Logout(HttpContext context)
    {
        context.Response.Cookies.Delete(AuthConstants.REFRESH_TOKEN_COOKIE_NAME);
    }

    public void CreateRefreshToken(string refreshToken, HttpContext context)
    {
        var options = new CookieOptions()
        {
            HttpOnly = true
        };

        context.Response.Cookies.Append(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, refreshToken, options);
    }
}
