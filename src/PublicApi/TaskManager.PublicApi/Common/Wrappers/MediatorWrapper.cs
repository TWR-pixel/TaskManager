using MediatR;
using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Common.Wrappers;

public sealed class MediatorWrapper(IMediator mediator) : IMediatorWrapper
{
    private readonly IMediator _mediator = mediator;

    public async Task<TResponse> SendAsync<TResponse>(RequestBase<TResponse> request,
                                                      CancellationToken cancellationToken = default) where TResponse : class
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
