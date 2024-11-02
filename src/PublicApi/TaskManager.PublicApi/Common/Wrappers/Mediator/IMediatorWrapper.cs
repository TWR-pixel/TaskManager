using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Common.Wrappers.Mediator;

public interface IMediatorWrapper
{
    public Task<TResponse> SendAsync<TResponse>
        (RequestBase<TResponse> request, CancellationToken cancellationToken = default) where TResponse : class;
}
