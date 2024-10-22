using Microsoft.Extensions.Caching.Memory;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Common.Email.Verifier;

public sealed class EmailVerifier(IMemoryCache cache, IUnitOfWork unitOfWork) : IEmailVerifier
{
    private readonly IMemoryCache _cache = cache;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Sets user's IsEmailConfirmed = true, and save into db
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="CodeNotVerifiedException"></exception>
    public async Task<UserEntity> Verify(string code, CancellationToken cancellationToken)
    {
        var isFound = _cache.TryGetValue(code, out string? value);

        if (!isFound) // if not found
            throw new CodeNotVerifiedException($"Code with value '{code}' doesn't exists, try resend code");

        var userId = int.Parse(value!);

        var userEntity = await _unitOfWork.Users.SingleOrDefaultAsync(new ReadUserByIdSpecification(userId), cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {userId} not found in cache. ");

        userEntity.IsEmailConfirmed = true;

        await _unitOfWork.Users.UpdateAsync(userEntity, cancellationToken);

        _cache.Remove(code);

        return userEntity;
    }
}
