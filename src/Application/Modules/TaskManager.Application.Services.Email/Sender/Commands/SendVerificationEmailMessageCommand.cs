using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendVerificationEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        string to,
                                                        ICodeStorage storage,
                                                        ILogger<EmailSenderService> logger) : ISendEmailMessageCommand
{
    private readonly ILogger<EmailSenderService> _logger = logger;
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly ICodeStorage _storage = storage;
    private readonly string _to = to;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Verification code generation");
        
        var code = RandomNumberGenerator.GetHexString(20);

        _logger.LogInformation("Generated code {VerificationCode}", code);

        _storage.Set(code, _to);

        _logger.LogInformation("Code has been saved to the repository with key {Code} and with email {To}", code, _to);
        _logger.LogInformation("Creating a verification message");

        using var msg = new MailMessage(_options.From, _to, "Verification email", $"<h2>{code}</h2>");
        msg.IsBodyHtml = true;

        _logger.LogInformation("Message was successfully created");
        _logger.LogInformation("Message has started sending from {From} to {To}", _options.From, _to);

        await _options.SmtpClient.SendMailAsync(msg, cancellationToken);

        _logger.LogInformation("Message was sent successfully!");
    }
}
