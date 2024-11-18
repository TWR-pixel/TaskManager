namespace TaskManager.Application.User.Common.Email;

public interface IEmailSender
{
    public Task SendCodeAsync(string to, string subject, string code, string body, CancellationToken cancellationToken);
    public Task SendRecoveryPasswordMessageAsync(string to, string code, CancellationToken cancellationToken);
    public Task SendVerificationMessageAsync(string to, string code, CancellationToken cancellationToken);
}
