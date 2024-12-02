using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.UserOrganization;
using TaskManager.Application.UserOrganization.Commands;
using TaskManager.Application.UserOrganization.Queries;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.UserOrganization;

[ApiController]
[Route("api/user-organizations")]
[Authorize]
public sealed class UserOrganizationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserOrganizationResponse>> Create(CreateUserOrganizationCommand command, CancellationToken cancellationToken)
        => await CreatedAtActionAsync(nameof(Create), command, cancellationToken);

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserOrganizationResponse>> GetById([FromQuery] GetUserOrganizationByIdQuery query, CancellationToken cancellationToken)
        => await OkAsync(query, cancellationToken);
}
