using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : CrudApiControllerBase
{
    public UserController(IMediatorFacade mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RegisterUserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return CreatedAtAction(nameof(RegisterUserAsync), response);
    }
}
