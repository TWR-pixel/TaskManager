﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskManager.Application.User;
using TaskManager.Application.User.Commands.DeleteById;
using TaskManager.Application.User.Commands.UpdateById;
using TaskManager.Application.User.Queries.GetById;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserController(IMediatorWrapper mediator, IOptions<JwtBearerOptions> jwtOptions) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetById([FromQuery] GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine(claim.Value + " " + claim.Issuer);
        }
        Console.WriteLine(HttpContext.Connection.RemoteIpAddress.ToString());
        Console.WriteLine(jwtOptions.Value.Audience);
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdateUserByIdResponse>> Update([FromBody] UpdateUserByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> DeleteById([FromBody] DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }



    #endregion
}
