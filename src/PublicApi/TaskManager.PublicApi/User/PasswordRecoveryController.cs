using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.User.Commands;
using TaskManager.Application.User.Queries;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.User;

[ApiController]
[Route("api/password-recovery")]
public sealed class PasswordRecoveryController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost("send-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SendPasswordRecoveryCodeResponse>> SendRecoveryCode([FromBody] SendPasswordRecoveryCodeQuery request,
                                                                                   CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("verify-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<VerifyPasswordRecoveryCodeResponse>> RecoverPassword([FromBody] VerifyPasswordRecoveryCodeCommand request,
                                                                        CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
