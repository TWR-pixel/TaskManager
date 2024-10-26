using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendRecoveryCodeEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        ICodeStorage storage,
                                                        string to) : ISendEmailMessageCommand
{
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly string _to = to;
    private readonly ICodeStorage _storage = storage;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        var code = RandomNumberGenerator.GetHexString(20);

        _storage.Set(code, _to);

        using var msg = new MailMessage(_options.From, _to, "Recovery code", $"<h1>{code}</h1>");

        await _options.SmtpClient.SendMailAsync(msg, cancellationToken);
    }
}
