using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;
using TaskManager.Application.Users.Requests.GetAllUsersTasksById;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetAllUserTasksByIdResponse>> GetById([FromQuery] GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
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
    #endregion
}
