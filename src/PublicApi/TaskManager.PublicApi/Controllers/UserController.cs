﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.User;
using TaskManager.Application.User.Commands;
using TaskManager.Application.User.Commands.DeleteById;
using TaskManager.Application.User.Commands.UpdateById;
using TaskManager.Application.User.Queries;
using TaskManager.Application.User.Queries.GetById;
using TaskManager.PublicApi.Common.Models;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserController(IMediatorWrapper mediator, IConfiguration configuration) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpPost("upload-user-profile-image")]
    public async Task<ActionResult<UserDto>> UploadUserProfileImage(UploadUserProfileImageModel model, CancellationToken cancellationToken = default)
    {
        var command = new UploadUserProfileImageCommand
        {
            FormFile = model.UserProfileImage,
            UserId = model.UserId,
            ProfileImageUrl = $"{Request.Scheme}://{Request.Host}/api/users"
        };

        UserDto? response = await Mediator.SendAsync(command, cancellationToken);

        return Ok(response);
    }

    [HttpGet("profile-image")]
    public async Task<ActionResult> GetUserProfileImage([FromQuery] GetUserProfileImageQuery query, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.SendAsync(query, cancellationToken);

        return File(response, $"image/{configuration["AvailableFileExtensionsForUploadingUserProfileImages"]}");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetById([FromQuery] GetUserByIdRequest request, CancellationToken cancellationToken)
    {
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
