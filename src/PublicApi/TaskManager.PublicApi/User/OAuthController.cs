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
        var url = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={googleOptions.ClientId}&scope=https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile&redirect_uri=https://localhost:7049/api/oauth/google-callback";

        return Redirect(url);
    }

    [HttpGet("google-callback")]
    public async Task<ActionResult<AccessTokenResponse>> GoogleCallback(string code, CancellationToken cancellationToken)
    {
        var command = new GoogleLoginCallbackCommand { Code = code };
        
        var response = await Mediator.SendAsync(command, cancellationToken);

        return CreatedAtAction(nameof(GoogleCallback), response);
    }
}
