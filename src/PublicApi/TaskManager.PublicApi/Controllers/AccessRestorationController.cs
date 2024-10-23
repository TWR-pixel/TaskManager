using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.RecoverPassword;
using TaskManager.Application.Users.Requests.SendPasswordRecoveryCode;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/access-restoration")]
public sealed class AccessRestorationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost("send-recovery-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SendPasswordRecoveryCodeResponse>> SendRecoveryCode([FromBody] SendPasswordRecoveryCodeRequest request,
                                                                                   CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("verify-recovery-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RecoverPasswordResponse>> RecoverPassword([FromBody] RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
