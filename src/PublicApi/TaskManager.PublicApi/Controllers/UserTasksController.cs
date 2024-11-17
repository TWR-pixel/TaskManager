using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.User.Queries.GetAllUserTasksById;
using TaskManager.Application.UserTask;
using TaskManager.Application.UserTask.Requests.Create;
using TaskManager.Application.UserTask.Requests.DeleteById;
using TaskManager.Application.UserTask.Requests.GetById;
using TaskManager.Application.UserTask.Requests.UpdateById;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/user-tasks")]
public sealed class UserTasksController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAllUserTasks([FromQuery] GetAllUserTasksByIdRequest request, CancellationToken cancellation)
    {
        var result = await Mediator.SendAsync(request, cancellation);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserTaskDto>> CreateTask([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return CreatedAtAction(nameof(CreateTask), result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdateTaskResponse>> UpdateTask([FromBody] UpdateTaskRequest request,
                                                                              CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
    #endregion

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DeleteUserTaskByIdResponse>> Delete([FromBody] DeleteUserTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetTaskByIdResponse>> GetById([FromQuery] GetTaskByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
