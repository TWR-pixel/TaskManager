using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace TaskManager.Application.Common.EmailSender;

public sealed class EmailSender
{
    public EmailSenderOptions Options { get; init; }
    private readonly SmtpClient _smtpClient;

    public EmailSender(EmailSenderOptions options)
    {
        Options = options;

        _smtpClient = new SmtpClient(options.SmtpAddress, options.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(options.From, options.Password)
        };
    }

    public void Send(MailMessage message)
    {
       _smtpClient.Send(message);
    }
}
