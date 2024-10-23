using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Requests.ResendCode;
using TaskManager.Application.Users.Requests.VerifyEmail;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

[ApiController]
[Route("api/email-verification")]
public class EmailVerificationController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<VerifyEmailResponse>> VerifyEmail([FromBody] VerifyEmailRequest request,
                                                                     CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost("resend-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ResendCodeResponse>> ResendCode([FromBody] ResendCodeRequest request,
                                                                   CancellationToken cancellationToken)
    {
        var response = await Mediator.SendAsync(request, cancellationToken);

        return Ok(response);
    }
}
