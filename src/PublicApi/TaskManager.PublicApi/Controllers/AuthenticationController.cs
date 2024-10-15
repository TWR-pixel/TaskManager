using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.GetNewUserAccessToken;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.PublicApi.Common;
using TaskManager.PublicApi.Common.Authentication;
using TaskManager.PublicApi.Common.Models.Response;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController : ApiControllerBase
{
    #region
    private readonly IUserSignInManager _userManager;

    public AuthenticationController(IMediatorFacade mediator, IUserSignInManager userManager) : base(mediator)
    {
        _userManager = userManager;
    }

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

        _userManager.Login(result.RefreshTokenString, HttpContext);

        return Ok(userLoginResponse);
    }
    // получать данные с сервера, которые по запросу: саму задачу, токен, колонку
    /// <summary>
    /// Returns new user token with refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("update-user-access-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetNewUserAccessTokenResponse>> UpdateUserAccessTokenByRefreshToken(CancellationToken cancellationToken)
    {
        var isRefreshTokenInCookie = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (!isRefreshTokenInCookie || string.IsNullOrWhiteSpace(value))
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
        var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (isRefreshTokenInRequest == false)
        {
            return Unauthorized("cookie with name 'RefreshToken' not found");
        }

        _userManager.Logout(HttpContext);

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

            var resp = (RegisterUserModelResponse)response;

            _userManager.CreateRefreshToken(response.RefreshTokenString, HttpContext);

            return CreatedAtAction(nameof(RegisterUser), resp);
        }
        catch (UserAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }
    #endregion
}
