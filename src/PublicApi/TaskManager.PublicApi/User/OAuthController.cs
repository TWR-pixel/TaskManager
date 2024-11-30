using Microsoft.AspNetCore.Mvc;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.User;

[ApiController]
[Route("api/oauth")]
public sealed class OAuthController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{

    [HttpGet("google-login")]
    public async Task<ActionResult> GoogleLogin()
    {
        var url = "https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id=945416324135-t9o7je9k0b07vf7qr2g600rpirht4qba.apps.googleusercontent.com&scope=https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile&redirect_uri=https://localhost:443/api/oauth/google-callback";

        return Redirect(url);
    }

    [HttpGet("google-callback")]
    public async Task<ActionResult> GoogleCallback()
    {

        return Ok();
    }
}
