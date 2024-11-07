using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.User.Commands.Register;
using TaskManager.Application.User.Common.AccessToken;
using TaskManager.Application.User.Queries.Login;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthenticationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    #region HTTP Methods
    /// <summary>
    /// Returns new accessToken 
    /// </summary>
    /// <param name="request"></param> without refresh token use it method
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AccessTokenResponse>> LoginUser([FromBody] LoginUserRequest request,
                                                                 CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }


    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest request,
                                                                       CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return CreatedAtAction(nameof(RegisterUser), response);
    }

    #endregion
}
