using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.RecoverPassword;
using TaskManager.Application.Users.Requests.SendPasswordRecoveryCode;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/password-recovery")]
public sealed class PasswordRecoveryController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost("send-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SendPasswordRecoveryCodeResponse>> SendRecoveryCode([FromBody] SendPasswordRecoveryCodeRequest request,
                                                                                   CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("verify-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RecoverPasswordResponse>> VerifyCode([FromBody] VerifyPasswordRecoveryCodeRequest request,
                                                                        CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
