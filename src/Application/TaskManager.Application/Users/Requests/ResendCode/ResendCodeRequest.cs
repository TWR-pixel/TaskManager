using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Users.Requests.ResendCode;

public sealed record ResendCodeRequest(string Email) : RequestBase<ResendCodeResponse>
{
}

public sealed record ResendCodeResponse : ResponseBase
{
}

public sealed class ResendCodeRequestHandler : RequestHandlerBase<ResendCodeRequest, ResendCodeResponse>
{
    private readonly IMemoryCache _cache;

    public ResendCodeRequestHandler(IUnitOfWork unitOfWork, IMemoryCache cache) : base(unitOfWork)
    {
        _cache = cache;
    }

    public override Task<ResendCodeResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var verificationCode = RandomNumberGenerator.GetHexString(20);



        _cache.Set(verificationCode, request);

        throw new NotImplementedException();
    }
}
