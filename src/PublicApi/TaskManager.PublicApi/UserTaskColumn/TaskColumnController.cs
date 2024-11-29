using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.UserTaskColumn;
using TaskManager.Application.UserTaskColumn.Requests.Commands;
using TaskManager.Application.UserTaskColumn.Requests.Queries.GetAllUserTasksColumnsByIdRequest;
using TaskManager.Application.UserTaskColumn.Requests.Queries.GetAllUserTasksColumnsByIdRequest.Dtos;
using TaskManager.Application.UserTaskColumn.Requests.Queries.GetById;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.UserTaskColumn;

[ApiController]
[Authorize]
[Route("api/task-columns")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public sealed class TaskColumnController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserTaskColumnDto>> Create(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Created(nameof(Create), result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DeleteTaskColumnByIdResponse>> Delete(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAllUserTaskColumnsByIdWithTasksResponse>> GetAllUserTaskColumns([FromQuery] GetAllUserTaskColumnsByIdWithTasksRequest request,
                                                                                                      CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserTaskColumnDto>> GetTaskColumnById([FromQuery] GetTaskColumnByIdRequest request,
                                                                                 CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UpdateTaskColumnByIdResponse>> UpdateTaskColumn([FromBody] UpdateTaskColumnByIdRequest request,
                                                                                   CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    #endregion
}
