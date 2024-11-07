using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Cryptography;
using TaskManager.Application.User.Common.Email.Code.Storage;
using TaskManager.Application.User.Common.Email.Options;

namespace TaskManager.Application.User.Common.Email.Sender;

public sealed class EmailSenderService(IOptions<EmailSenderOptions> options,
                                       ILogger<EmailSenderService> logger,
                                       ICodeStorage codeStorage) : IEmailSenderService
{
    private readonly EmailSenderOptions _options = options.Value;
    private readonly SmtpClient _client = options.Value.SmtpClient;

    public async Task SendCodeAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var code = RandomNumberGenerator.GetHexString(20);

        codeStorage.Set(code, to);
        using var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Administration task-manager", _options.From));
        msg.To.Add(new MailboxAddress(to, to));
        msg.Subject = subject;
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<h1>{code}</h1> <br> {body}"
        };

        logger.LogInformation("Message was successfully created");

        logger.LogInformation("Message has started sending from {From} to {To}", _options.From, to);

        _client.Connect(_options.Host, _options.Port, true, cancellationToken);
        _client.Authenticate(_options.From, _options.Password, cancellationToken);

        await _client.SendAsync(msg, cancellationToken);

        logger.LogInformation("Message was sent successfully!");

        await _client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendVerificationMessageAsync(string to, CancellationToken cancellationToken)
    {
        await SendCodeAsync(to, "Verification code", "Use this code to confirm email address", cancellationToken);
    }

    public async Task SendRecoveryPasswordMessageAsync(string to, CancellationToken cancellationToken)
    {
        await SendCodeAsync(to, "Password recovery code", "If you didn't asked recovery password, ignore it", cancellationToken);
    }
}
