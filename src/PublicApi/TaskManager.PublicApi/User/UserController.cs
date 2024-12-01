using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    //[HttpPost("google-login")]
    //public ActionResult GoogleLogin()
    //{
    //    var url = "https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id=945416324135-t9o7je9k0b07vf7qr2g600rpirht4qba.apps.googleusercontent.com&scope=https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile&redirect_uri=localhost";
        

    //    return Challenge();
    //}

    //[HttpGet("google-callback")]
    //public ActionResult GoogleCallback()
    //{

    //    return Ok();
    //}

    [HttpPost("upload-user-profile-image")]
    public async Task<ActionResult<UserDto>> UploadUserProfileImage(UploadUserProfileImageModel model, CancellationToken cancellationToken = default)
    {
        var command = new UploadUserProfileImageCommand
        {
            FormFile = model.UserProfileImage,
            UserId = model.UserId,
            ProfileImageLink = $"{Request.Scheme}://{Request.Host}/api/users"
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
    public async Task<ActionResult<UserDto>> DeleteById([FromBody] DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }


    #endregion
}
