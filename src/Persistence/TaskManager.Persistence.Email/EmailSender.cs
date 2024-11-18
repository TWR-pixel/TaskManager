using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using TaskManager.Application.User.Common.Email;

namespace TaskManager.Persistence.Email;

public sealed class EmailSender(IOptions<EmailSenderOptions> options,
                                       ILogger<EmailSender> logger) : IEmailSender
{
    private readonly EmailSenderOptions _options = options.Value;
    private readonly SmtpClient _client = options.Value.SmtpClient;
    
    public async Task SendCodeAsync(string to, string subject, string code, string body, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started sending mail from {from} to {to}", _options.From, to);

        using var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Administration task-manager", _options.From));
        msg.To.Add(new MailboxAddress(to, to));
        msg.Subject = subject;
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<h1>{code}</h1> <br> {body}"
        };
        logger.LogTrace("Message was successfully created");

        logger.LogTrace("Message has started sending from {From} to {To}", _options.From, to);
        _client.Connect(_options.Host, _options.Port, true, cancellationToken);
        _client.Authenticate(_options.From, _options.Password, cancellationToken);

        await _client.SendAsync(msg, cancellationToken);
        logger.LogInformation("Message was sent successfully!");

        await _client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendVerificationMessageAsync(string to, string code, CancellationToken cancellationToken)
    {
        await SendCodeAsync(to, "Verification code", code, "Use this code to confirm email address", cancellationToken);
    }

    public async Task SendRecoveryPasswordMessageAsync(string to, string code, CancellationToken cancellationToken)
    {
        await SendCodeAsync(to, "Password recovery code", code, "If you didn't asked recovery password, ignore it", cancellationToken);
    }
}
