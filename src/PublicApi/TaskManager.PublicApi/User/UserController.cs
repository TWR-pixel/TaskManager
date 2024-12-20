﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.User;
using TaskManager.Application.User.Commands;
using TaskManager.Application.User.Queries;
using TaskManager.PublicApi.Common.Controllers;
using TaskManager.PublicApi.Common.Models;

namespace TaskManager.PublicApi.User;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserController(IMediatorWrapper mediator, IConfiguration configuration) : ApiControllerBase(mediator)
{
    #region HTTP methods

    [HttpGet("all-user-organizations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetAllUserOrganizations(GetAllUserOrganizationsQuery query, CancellationToken cancellationToken)
        => await OkAsync(query, cancellationToken);

    [HttpPost("upload-user-profile-image")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserDto>> UploadUserProfileImage(UploadUserProfileImageModel model, CancellationToken cancellationToken = default)
    {
        var command = new UploadUserProfileImageCommand
        {
            FormFile = model.UserProfileImage,
            UserId = model.UserId,
            ProfileImageLink = $"{Request.Scheme}://{Request.Host}/api/users"
        };

        UserDto? response = await Mediator.SendAsync(command, cancellationToken);

        return CreatedAtAction(nameof(UploadUserProfileImage), response);
    }

    [HttpGet("profile-image")]
    [AllowAnonymous]
    public async Task<ActionResult> GetUserProfileImage([FromQuery] GetUserProfileImageQuery query, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.SendAsync(query, cancellationToken);

        return File(response, $"image/{configuration["AvailableFileExtensionsForUploadingUserProfileImages"]}");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetById([FromQuery] GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdateUserByIdResponse>> Update([FromBody] UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> DeleteById([FromBody] DeleteByIdCommandBase<UserDto> request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }


    #endregion
}
