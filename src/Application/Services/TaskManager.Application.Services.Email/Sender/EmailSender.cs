using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Sender.Options;

namespace TaskManager.Application.Modules.Email.Sender;

public sealed class EmailSender(IOptions<EmailSenderOptions> options, ICodeStorage codeStorage) : IEmailSender
{
    public EmailSenderOptions Options { get; init; } = options.Value;

    private readonly ICodeStorage _codeStorage = codeStorage;
    private readonly SmtpClient _smtpClient = options.Value.SmtpClient;

    public async Task SendMailAsync(MailMessage message, CancellationToken cancellationToken)
    {
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }

    public async Task SendVerificationCodeAsync(string from, string to, CancellationToken cancellationToken)
    {
        var verificationCode = RandomNumberGenerator.GetHexString(20, false);

        _codeStorage.Set(verificationCode, to);

        var msg = new MailMessage(from, to, "Email verification", $"Verification code <h2>{verificationCode}</h2>")
        {
            IsBodyHtml = true
        };

        await SendMailAsync(msg, cancellationToken);
    }
}
