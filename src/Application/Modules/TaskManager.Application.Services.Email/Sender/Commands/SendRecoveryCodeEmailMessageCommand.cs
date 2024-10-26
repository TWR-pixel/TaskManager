using Microsoft.Extensions.Options;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Messages;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendRecoveryCodeEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        ICodeStorage storage,
                                                        string to,
                                                        IMailMessageFactory messageFactory) : ISendEmailMessageCommand
{
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly string _to = to;
    private readonly ICodeStorage _storage = storage;
    private readonly IMailMessageFactory _messageFactory = messageFactory;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        await _options.SmtpClient.VerifyAsync(_to, cancellationToken);

        using var msg = _messageFactory.CreateRecoveryPassword(_to);

        await _options.SmtpClient.SendAsync(msg, cancellationToken);
    }
}
