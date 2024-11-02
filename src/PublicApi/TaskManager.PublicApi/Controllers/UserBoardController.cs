using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Authorize]
[Route("api/user-boards")]
public sealed class UserBoardController : ApiControllerBase
{

    public UserBoardController(IMediatorWrapper mediator) : base(mediator)
    {
    }

    
}
