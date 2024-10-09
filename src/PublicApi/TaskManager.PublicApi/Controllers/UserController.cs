using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;
using TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.DeleteUserByIdRequest;
using TaskManager.Application.Users.Requests.GetUserByIdRequest;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
using TaskManager.Application.Users.Requests.UpdateUserByIdRequest;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAllUserTasksByIdResponse>> GetById([FromQuery] GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Mediator.SendAsync(request, cancellationToken);

            return Ok(result);
        }
        catch (EntityNotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
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
    public async Task<ActionResult<DeleteUserByIdResponse>> DeleteById([FromBody] DeleteUserByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
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
