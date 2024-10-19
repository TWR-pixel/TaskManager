using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.Authenticate;
using TaskManager.Application.Users.Requests.Register;
using TaskManager.PublicApi.Common.Models.Response;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    #region
    /// <summary>
    /// Returns new accessToken with refresh token in httpOnly cookies, if refresh token not found, it creates in cookies
    /// </summary>
    /// <param name="request"></param> without refresh token use it method
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserLoginResponse>> LoginUser([FromBody] AuthenticateUserRequest request,
                                                                                      CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        var userLoginResponse = (UserLoginResponse)result;

        return Ok(userLoginResponse);
    }
    //// получать данные с сервера, которые по запросу: саму задачу, токен, колонку
    ///// <summary>
    ///// Returns new user token with refresh token
    ///// </summary>
    ///// <returns></returns>
    //[HttpPost("update-user-access-token")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //public async Task<ActionResult<GetUserAccessTokenResponse>> UpdateUserAccessTokenByRefreshToken(CancellationToken cancellationToken)
    //{
    //    var isRefreshTokenInCookie = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

    //    if (!isRefreshTokenInCookie || string.IsNullOrWhiteSpace(value))
    //        return Unauthorized();

    //    var mediatorRequest = new GetNewAccessTokenRequest() { RefreshToken = value };

    //    var result = await Mediator.SendAsync(mediatorRequest, cancellationToken);

    //    return Ok(result);
    //}

    //[HttpPost("logout")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public ActionResult Logout()
    //{
    //    var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

    //    if (isRefreshTokenInRequest == false)
    //    {
    //        return Unauthorized("cookie with name 'RefreshToken' not found");
    //    }

    //    _userManager.Logout(HttpContext);

    //    return Ok();
    //}

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterUserModelResponse>> RegisterUser([FromBody] RegisterUserRequest request,
                                                             CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        var resp = (RegisterUserModelResponse)response;

        return CreatedAtAction(nameof(RegisterUser), resp);
    }
    #endregion
}
