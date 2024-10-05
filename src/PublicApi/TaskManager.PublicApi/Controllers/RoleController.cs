using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Role.Requests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RoleController : CrudApiControllerBase
{
    public RoleController(IMediatorFacade mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<AddAndSAveRoleResponse>> Create(AddAndSaveRoleRequest request, CancellationToken cancellation)
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
}
