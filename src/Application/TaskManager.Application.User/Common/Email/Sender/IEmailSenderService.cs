namespace TaskManager.Application.User.Common.Email.Sender;

public interface IEmailSenderService
{
    public Task SendCodeAsync(string to, string subject, string body, CancellationToken cancellationToken);
    public Task SendRecoveryPasswordMessageAsync(string to, CancellationToken cancellationToken);
    public Task SendVerificationMessageAsync(string to, CancellationToken cancellationToken);
}
