using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.User.Commands;
using TaskManager.Application.User.Queries;
using TaskManager.PublicApi.Common.Controllers;

namespace TaskManager.PublicApi.User;

[ApiController]
[Route("api/email-verification")]
public class EmailVerificationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AccessTokenResponse>> VerifyEmail([FromBody] VerifyEmailVerificationCodeCommand request,
                                                                     CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("resend-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ResendVerificationCodeResponse>> ResendCode([FromBody] ResendVerificationCodeQuery request,
                                                                   CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
