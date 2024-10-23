using System.Net.Mail;
using TaskManager.Application.Modules.Email.Sender.Options;

namespace TaskManager.Application.Modules.Email.Sender;

public interface IEmailSender
{
    public EmailSenderOptions Options { get; init; }
    public Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default);
    public Task SendVerificationCodeAsync(string to, CancellationToken cancellationToken = default);
    public Task SendRecoveryCodeAsync(string to, CancellationToken cancellationToken = default);
}
