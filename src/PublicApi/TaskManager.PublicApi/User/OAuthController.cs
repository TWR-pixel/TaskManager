using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth.OAuth.Google;
using TaskManager.Application.User.Commands;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.User;

[ApiController]
[Route("api/oauth")]
public sealed class OAuthController(IMediatorWrapper mediator, IOptions<GoogleOAuthOptions> oauthOptions) : ApiControllerBase(mediator)
{
    private readonly GoogleOAuthOptions googleOptions = oauthOptions.Value;

    [HttpGet("google-login")]
    public async Task<ActionResult> GoogleLogin()
    {
        var url = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={googleOptions.ClientId}&scope=https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile&redirect_uri={googleOptions.ServerRedirectUri}";

        return Redirect(url);
    }

    [HttpGet("google-callback")]
    public async Task<ActionResult<AccessTokenResponse>> GoogleCallback(string code, CancellationToken cancellationToken)
    {
        var command = new GoogleLoginCallbackCommand { Code = code };

        var response = await Mediator.SendAsync(command, cancellationToken);

        return Redirect($"{googleOptions.ClientRedirectUri}?access_token={response.AccessTokenString}");
    }

    //[HttpGet("google-login")]
    //public IActionResult GoogleLogin()
    //{
    //    return Challenge(GoogleDefaults.AuthenticationScheme);
    //}

    //[HttpGet("google-callback")] //обработка ответа от Google
    //public async Task<IActionResult> GoogleCallbackAsync()
    //{
    //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

    //    if (result.Succeeded)
    //    {
    //        // Пользователь успешно авторизован
    //        var accessToken = result.Properties.GetTokenValue("access_token"); //Получение access token
    //        var refreshToken = result.Properties.GetTokenValue("refresh_token"); //Получение refresh token
    //        var expiresAt = result.Properties.GetTokenValue("expires_at");
    //        var idToken = result.Properties.GetTokenValue("id_token");

    //        // Здесь можно сохранить токен в базе данных или использовать его для доступа к API Google.
    //        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresAt = expiresAt, IdToken = idToken });
    //    }
    //    else
    //    {
    //        // Авторизация не удалась
    //        return Unauthorized();
    //    }
    //}
}
