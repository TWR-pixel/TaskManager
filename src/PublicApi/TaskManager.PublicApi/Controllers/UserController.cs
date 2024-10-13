using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;
using TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;
using TaskManager.Application.Users.Requests.DeleteUserByIdRequest;
using TaskManager.Application.Users.Requests.GetUserByIdRequest;
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



    #endregion
}
