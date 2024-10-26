using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Role.Requests;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/roles")]
public sealed class RoleController(IMediatorWrapper mediator, ILogger<RoleController> logger) : ApiControllerBase(mediator)
{
    private readonly ILogger<RoleController> _logger = logger;

    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateRoleResponse>> Create([FromBody] CreateRoleRequest request,
                                                               CancellationToken cancellation)
    {
        _logger.LogInformation("Requested to create a new role {Name}", request.Name);

        var response = await Mediator.SendAsync(request, cancellation);

        return CreatedAtAction(nameof(Create), response);
    }

    #endregion
}
