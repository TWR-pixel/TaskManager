using MediatR;
using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Common;

public sealed class MediatorFacade : IMediatorFacade
{
    private readonly IMediator _mediator;

    public MediatorFacade(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> SendAsync<TResponse>
        (RequestBase<TResponse> request, CancellationToken cancellationToken = default) where TResponse : class
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
