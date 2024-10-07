using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.TaskColumns.Requests.AddAndSaveTaskColumnRequests;
using TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;
using TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/task-columns")]
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

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DeleteTaskColumnByIdResponse>> Delete(DeleteTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAllUserTaskColumnsByIdResponse>> GetAllUserTaskColumns([FromQuery] GetAllUserTaskColumnsByIdRequest request,
                                                                                                CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
    #endregion
}
