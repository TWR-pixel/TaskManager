using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.TaskColumns.Requests.CreateTaskColumnRequest;
using TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;
using TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest;
using TaskManager.Application.TaskColumns.Requests.GetTaskColumnByIdRequest;
using TaskManager.Application.TaskColumns.Requests.UpdateTaskColumnByIdRequest;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/task-columns")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public sealed class TaskColumnController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateTaskColumnResponse>> Create(CreateTaskColumnRequest request,
                                                                         CancellationToken cancellationToken)
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
    public async Task<ActionResult<GetAllUserTaskColumnsByIdResponse>> GetAllUserTaskColumns([FromQuery] GetAllUserTaskColumnsByIdRequest request,
                                                                                                CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetTaskColumnByIdResponse>> GetTaskColumnById([FromQuery] GetTaskColumnByIdRequest request,
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

    //[HttpPut]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<UpdateTaskColumnByIdResponse>> 
    #endregion
}
