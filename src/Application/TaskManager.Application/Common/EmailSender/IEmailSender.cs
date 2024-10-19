using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace TaskManager.Application.Common.EmailSender;

public interface IEmailSender
{
    public EmailSenderOptions Options { get; init; }
    public Task SendMailAsync(MailMessage message, CancellationToken cancellationToken);
    public void SendAsync(MailMessage message, CancellationToken cancellationToken);
}
