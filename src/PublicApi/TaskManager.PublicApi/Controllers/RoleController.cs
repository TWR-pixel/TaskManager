using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Role.Requests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/roles")]
public sealed class RoleController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateRoleResponse>> Create([FromBody] CreateRoleRequest request,
                                                                   CancellationToken cancellation)
    {
        var response = await Mediator.SendAsync(request, cancellation);

        return CreatedAtAction(nameof(Create), response);
    }

    //[HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<ActionResult> GetByIdAsync()
    //{
    //    return Ok();
    //}
    #endregion
}
