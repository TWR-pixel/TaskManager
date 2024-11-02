namespace TaskManager.Application.Modules.Email.Verifier;

public interface IEmailVerifierService
{
    public Task<EmailVerifierServiceResponse> Verify(string email, CancellationToken cancellationToken);
}
