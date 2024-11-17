using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Controllers;

public abstract class ApiControllerBase(IMediatorWrapper mediator) : ControllerBase
{
    private readonly IMediatorWrapper _mediator = mediator;

    protected IMediatorWrapper Mediator => _mediator;

    protected virtual async Task<TResponse> SendAsync<TResponse>
        (RequestBase<TResponse> request, CancellationToken cancellationToken = default) where TResponse : class
    {
        var response = await _mediator.SendAsync(request, cancellationToken);

        return response;
    }
}
