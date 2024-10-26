using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Messages;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendRecoveryCodeEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        string to,
                                                        IMailMessageFactory messageFactory) : ISendEmailMessageCommand
{
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly string _to = to;
    private readonly IMailMessageFactory _messageFactory = messageFactory;
    private readonly SmtpClient _client = Options.Value.SmtpClient;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        using var msg = _messageFactory.CreateRecoveryPassword(_to);

        _client.Connect(_options.Host, _options.Port, true, cancellationToken);
        _client.Authenticate(_options.From, _options.Password, cancellationToken);

        await _options.SmtpClient.SendAsync(msg, cancellationToken);

        await _client.DisconnectAsync(true, cancellationToken);
    }
}
