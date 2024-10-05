using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;
using TaskManager.Application.Users.Requests.RegisterUserRequests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UserController : CrudApiControllerBase
{
    public UserController(IMediatorFacade mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest request,
                                                                       CancellationToken cancellationToken)
    {
        try
        {
            var response = await Mediator.SendAsync(request, cancellationToken);

            return CreatedAtAction(nameof(RegisterUser), response);
        }
        catch (UserAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }




}
