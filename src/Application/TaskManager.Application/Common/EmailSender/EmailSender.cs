using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace TaskManager.Application.Common.EmailSender;

public sealed class EmailSender : IEmailSender
{
    public EmailSenderOptions Options { get; init; }

    private readonly SmtpClient _smtpClient;

    public EmailSender(IOptions<EmailSenderOptions> options)
    {
        Options = options.Value;
        _smtpClient = options.Value.SmtpClient;
    }

    public async Task SendMailAsync(MailMessage message, CancellationToken cancellationToken)
    {
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }

    public void SendAsync(MailMessage message, CancellationToken cancellationToken)
    {
        _smtpClient.SendAsync(message, cancellationToken);
    }
}
