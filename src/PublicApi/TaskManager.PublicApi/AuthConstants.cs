using TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;

namespace TaskManager.PublicApi;

public static class AuthConstants
{
    public static string AUTH_REFRESH_TOKEN_COOKIE_NAME = "RefreshToken" + new JwtAuthenticationOptions().Issuer;
}
