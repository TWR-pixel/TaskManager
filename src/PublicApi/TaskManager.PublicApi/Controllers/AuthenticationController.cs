using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Security.Authentication;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.GetNewUserAccessToken;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
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

    public AuthenticationController(IMediatorFacade mediator) : base(mediator)
    {
        _userManager = new UserSignInManager(HttpContext);
    }

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

        var userLoginResponse = new UserLoginResponse()
        {
            AccessTokenString = result.AccessTokenString,
            RoleId = result.RoleId,
            RoleName = result.RoleName,
            UserId = result.UserId,
            UserName = result.UserName,
        };

        _userManager.Login(result.RefreshTokenString);


        return Ok(userLoginResponse);
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
        var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

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
        var isRefreshTokenInRequest = Request.Cookies.TryGetValue(AuthConstants.REFRESH_TOKEN_COOKIE_NAME, out string? value);

        if (isRefreshTokenInRequest == false)
        {
            return Unauthorized("cookie with name 'RefreshToken' not found");
        }

        _userManager.Logout();

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
            
            _userManager.CreateRefreshToken(response.RefreshTokenString);

            return CreatedAtAction(nameof(RegisterUser), resp);
        }
        catch (UserAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }
    #endregion
}
