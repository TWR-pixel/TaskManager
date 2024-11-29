using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Role;
using TaskManager.Application.Role.Commands;

namespace TaskManager.PublicApi.Role;

[ApiController]
[Route("api/roles")]
public sealed class RoleController(IMediatorWrapper mediator, ILogger<RoleController> logger) : ControllerBase
{
    private readonly ILogger<RoleController> _logger = logger;

    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleCommand request,
                                                               CancellationToken cancellation)
    {
        _logger.LogInformation("Requested to create a new role {Title}", request.Name);

        var response = await mediator.SendAsync(request, cancellation);

        return CreatedAtAction(nameof(Create), response);
    }

    #endregion
}
