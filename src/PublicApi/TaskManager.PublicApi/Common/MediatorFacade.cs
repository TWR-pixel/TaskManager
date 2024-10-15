using MediatR;
using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Common;

public sealed class MediatorFacade(IMediator mediator) : IMediatorFacade
{
    private readonly IMediator _mediator = mediator;

    public async Task<TResponse> SendAsync<TResponse>(RequestBase<TResponse> request,
                                                      CancellationToken cancellationToken = default) where TResponse : ResponseBase
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
