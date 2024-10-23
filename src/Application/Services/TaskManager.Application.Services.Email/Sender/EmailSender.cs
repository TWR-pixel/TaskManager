using Microsoft.Extensions.Options;
using System.Net.Mail;
using TaskManager.Application.Modules.Email.Message;
using TaskManager.Application.Modules.Email.Sender.Options;

namespace TaskManager.Application.Modules.Email.Sender;

public sealed class EmailSender(IOptions<EmailSenderOptions> options,
                                IVerificationMessageFactory verificationFactory,
                                IRecoveryPasswordMessageFactory recoveryCodeFactory) : IEmailSender
{
    public EmailSenderOptions Options { get; init; } = options.Value;

    private readonly IVerificationMessageFactory _verificationFactory = verificationFactory;
    private readonly IRecoveryPasswordMessageFactory _recoveryCodeFactory = recoveryCodeFactory;
    private readonly SmtpClient _smtpClient = options.Value.SmtpClient;

    public async Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default)
    {
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }

    public async Task SendVerificationCodeAsync(string to, CancellationToken cancellationToken = default)
    {
        var msg = _verificationFactory.Create(to);

        await SendMailAsync(msg, cancellationToken);
    }

    public async Task SendRecoveryCodeAsync(string to, CancellationToken cancellationToken = default)
    {
        var msg = _recoveryCodeFactory.Create(to);

        await SendMailAsync(msg, cancellationToken);
    }
}
