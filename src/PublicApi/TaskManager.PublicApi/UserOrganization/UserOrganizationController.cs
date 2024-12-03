using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Requests.Commands;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserOrganizationResponse>> GetById([FromQuery] GetUserOrganizationByIdQuery query, CancellationToken cancellationToken)
        => await OkAsync(query, cancellationToken);

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserOrganizationResponse>> DeleteById(DeleteByIdCommandBase<UserOrganizationResponse> command, CancellationToken cancellationToken)
        => await OkAsync(command, cancellationToken);

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserOrganizationResponse>> UpdateById(UpdateUserOrganizationCommand command, CancellationToken cancellationToken)
        => await OkAsync(command, cancellationToken);
}
