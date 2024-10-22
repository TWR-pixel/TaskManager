using System.Net.Mail;

namespace TaskManager.Application.Common.Email.Sender;

public interface IEmailSender
{
    public EmailSenderOptions Options { get; init; }
    public Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default);
    public Task SendVerificationCodeAsync(string To, CancellationToken cancellationToken = default);
}
