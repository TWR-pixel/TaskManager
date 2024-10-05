using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController(IMediatorFacade mediator) : CrudApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthenticateUserByEmailResponse>> AuthenticateUser([FromQuery] AuthenticateUserRequestByEmail request,
                                                                                      CancellationToken cancellationToken)
    {
        var result = await Mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
