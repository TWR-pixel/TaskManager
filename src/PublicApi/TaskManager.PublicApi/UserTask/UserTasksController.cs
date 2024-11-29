using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.User.Queries.GetAllUserTasksById;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTask.Requests.Commands;
using TaskManager.Application.UserTask.Requests.Queries;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.UserTask;

[ApiController]
[Authorize]
[Route("api/user-tasks")]
public sealed class UserTasksController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAllUserTasks([FromQuery] GetAllUserTasksByIdQuery request, CancellationToken cancellation)
    {
        var result = await Mediator.SendAsync(request, cancellation);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserTaskDto>> CreateTask([FromBody] CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return CreatedAtAction(nameof(CreateTask), result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdateUserTaskResponse>> UpdateTask([FromBody] UpdateTaskCommand request,
                                                                              CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
    #endregion

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DeleteUserTaskByIdResponse>> Delete([FromBody] DeleteUserTaskByIdCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetTaskByIdResponse>> GetById([FromQuery] GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
