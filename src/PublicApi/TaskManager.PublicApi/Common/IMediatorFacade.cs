using MediatR;
using TaskManager.Application.Common.Requests;

namespace TaskManager.PublicApi.Common;

public interface IMediatorFacade
{
    public Task<TResponse> SendAsync<TResponse>
        (RequestBase<TResponse> request, CancellationToken cancellationToken = default) where TResponse : class;
}
