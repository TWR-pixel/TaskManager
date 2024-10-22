using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Common.Email.Verifier;

public interface IEmailVerifier
{
    /// <summary>
    /// Sets user's IsEmailConfirmed = true, and save into db
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    public Task<UserEntity> Verify(string code, CancellationToken cancellationToken);
}