using TaskManager.Application.Modules.Email.Sender.Commands;

namespace TaskManager.Application.Modules.Email.Sender;

public interface IEmailSenderService
{
    public Task SendAsync(ISendEmailMessageCommand message, CancellationToken cancellationToken);
    public Task SendRecoveryCodeAsync(string to, CancellationToken cancellationToken);
    public Task SendVerificationCodeAsync(string to, CancellationToken cancellationToken);
}
