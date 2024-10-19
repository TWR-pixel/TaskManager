using Microsoft.Extensions.Caching.Memory;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.ConfirmRegistration;

public sealed record ConfirmRegistrationRequest : RequestBase<ConfirmRegistrationResponse>
{
    public required int UserId { get; set; }
    public required string Code { get; set; }
}

public sealed record ConfirmRegistrationResponse : ResponseBase
{
}

public sealed class ConfirmRegistrationRequestHandler
    : RequestHandlerBase<ConfirmRegistrationRequest, ConfirmRegistrationResponse>
{
    private readonly IMemoryCache _cache;

    public ConfirmRegistrationRequestHandler(IMemoryCache cache, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _cache = cache;
    }

    public override async Task<ConfirmRegistrationResponse> Handle(ConfirmRegistrationRequest request, CancellationToken cancellationToken)
    {
        var isFound = _cache.TryGetValue(request.UserId.ToString(), out string? code);

        if (!isFound) // if not found
            throw new CodeNotFoundException($"Code with key '{request.UserId}' not found, try resend code");

        if (request.Code != code)
            throw new NotRightCodeException($"Code from request '{request.Code}' not right.");

        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found.");

        userEntity.IsEmailConfirmed = true;

        await UnitOfWork.Users.UpdateAsync(userEntity, cancellationToken);

        _cache.Remove(request.UserId.ToString());

        return new ConfirmRegistrationResponse();
    }
}
