using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskManager.Application.Role;
using TaskManager.Application.Role.Requests;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/roles")]
public sealed class RoleController(IMediatorWrapper mediator, ILogger<RoleController> logger) : ControllerBase
{
    private readonly ILogger<RoleController> _logger = logger;

    #region HTTP methods
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleRequest request,
                                                               CancellationToken cancellation)
    {
        _logger.LogInformation("Requested to create a new role {Title}", request.Name);



        var response = await mediator.SendAsync(request, cancellation);

        return CreatedAtAction(nameof(Create), response);
    }

    #endregion
}
