using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.GetNewUserAccessToken;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
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
        var isRefreshTokenInRequest= Request.Cookies.TryGetValue(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (!isRefreshTokenInRequest || string.IsNullOrWhiteSpace(value))
            return Unauthorized();

        var mediatorRequest = new GetNewUserAccessTokenRequest() { RefreshToken = value };

        var result = await Mediator.SendAsync(mediatorRequest, cancellationToken);

        return Ok(result);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Logout()
    {
        var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (isRefreshTokenInRequest == false)
        {
            return NotFound("cookie with name 'RefreshToken' not found");
        }

        Response.Cookies.Delete(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME);

        return Ok();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterUserModelResponse>> RegisterUser([FromBody] RegisterUserRequest request,
                                                             CancellationToken cancellationToken)
    {
        try
        {
            var response = await Mediator.SendAsync(request, cancellationToken);

            var resp = new RegisterUserModelResponse()
            {
                AccessTokenString = response.AccessTokenString,
                RoleId = response.RoleId,
                RoleName = response.RoleName,
                UserId = response.UserId,
                Username = response.Username,
            };

            var options = new CookieOptions()
            {
                HttpOnly = true
            };

            Response.Cookies.Append(AuthConstants.AUTH_REFRESH_TOKEN_COOKIE_NAME, response.RefreshTokenString, options);

            return CreatedAtAction(nameof(RegisterUser), resp);
        }
        catch (UserAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }
    #endregion
}
