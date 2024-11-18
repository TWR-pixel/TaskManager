using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.UserBoard;
using TaskManager.Application.UserBoard.Queries.GetAllById;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/user-boards")]
public sealed class UserBoardController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserBoardDto>>> GetAllByOwnerId(GetAllUserBoardsByOwnerIdRequest request, CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
