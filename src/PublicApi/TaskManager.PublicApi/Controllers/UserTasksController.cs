using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Requests.AddAndSaveTaskRequest;
using TaskManager.Application.Tasks.Requests.UpdatePatchAndSaveTaskRequest;
using TaskManager.Application.Users.Requests.GetAllUsersTasksById;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class UserTasksController(IMediatorFacade mediator) : CrudApiControllerBase(mediator)
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAllUserTasksByIdResponse>> GetAllUserTasks(
        [FromQuery] GetAllUserTasksByIdRequest request, CancellationToken cancellation)
    {
        var result = await Mediator.SendAsync(request, cancellation);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AddAndSaveTaskResponse>> CreateTask([FromBody] AddAndSaveTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return CreatedAtAction(nameof(CreateTask), result);
    }

    [HttpPatch]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdatePatchAndSaveTaskResponse>> PatchTask([FromBody] UpdatePatchAndSaveTaskRequest request,
                                                                              CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
