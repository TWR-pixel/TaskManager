namespace TaskManager.Application.User.Common.Email.Verifier;

public interface IEmailVerifierService
{
    public Task<EmailVerifierServiceResponse> Verify(string email, CancellationToken cancellationToken);
}
