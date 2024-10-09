using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.GetNewUserAccessToken;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
    #region

    /// <summary>
    /// Returns new accessToken with refresh token in httpOnly cookies, if refresh token not found, it creates in cookies
    /// </summary>
    /// <param name="request"></param> without refresh token use it method
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserLoginResponse>> LoginUser([FromBody] AuthenticateUserRequest request,
                                                                                      CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        var resultResponse = new UserLoginResponse()
        {
            AccessTokenString = result.AccessTokenString,
            RoleId = result.RoleId,
            RoleName = result.RoleName,
            UserId = result.UserId,
            UserName = result.UserName,
        };

        var getTokenValue = HttpContext.Request.Cookies.TryGetValue(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, out string? tokenValue);

        if (tokenValue != result.RefreshTokenString)
        {
            Response.Cookies.Delete(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME);

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true
            }; 

            Response.Cookies.Append(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, result.RefreshTokenString, cookieOptions);
        }

        return Ok(resultResponse);

    }



    /// <summary>
    /// Returns new user token with refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("update-user-access-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetNewUserAccessTokenResponse>> UpdateUserAccessTokenByRefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies.TryGetValue(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (string.IsNullOrWhiteSpace(value))
            return Unauthorized();

        var mediatorRequest = new GetNewUserAccessTokenRequest() { RefreshToken = value };

        var result = await Mediator.SendAsync(mediatorRequest, cancellationToken);

        return Ok(result);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (isRefreshTokenInRequest == false)
        {
            return Unauthorized("Use http cookie with refresh token to logout from server.");
        }

        Response.Cookies.Delete(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME);

        return Ok();
    }
    #endregion
}
