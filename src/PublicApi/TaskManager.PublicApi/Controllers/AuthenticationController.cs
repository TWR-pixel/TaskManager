﻿using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.Login;
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
    public async Task<ActionResult<UserLoginResponse>> LoginUser([FromBody] LoginUserRequest request,
                                                                 CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        var userLoginResponse = (UserLoginResponse)result;

        return Ok(userLoginResponse);
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
