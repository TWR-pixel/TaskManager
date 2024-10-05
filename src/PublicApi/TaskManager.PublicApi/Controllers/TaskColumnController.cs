using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.TaskColumns.Requests.AddAndSaveTaskColumnRequests;
using TaskManager.Application.TaskColumns.Requests.DeleteTaskColumnRequests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class TaskColumnController(IMediatorFacade mediator) : CrudApiControllerBase(mediator)
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<AddAndSaveTaskColumnResponse>> Create(AddAndSaveTaskColumnRequest request,
                                                                         CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DeleteAndSaveTaskColumnByIdResponse>> Delete(DeleteAndSaveTaskColumnByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
