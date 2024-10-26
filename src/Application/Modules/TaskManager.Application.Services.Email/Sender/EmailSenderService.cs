using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Messages;
using TaskManager.Application.Modules.Email.Options;
using TaskManager.Application.Modules.Email.Sender.Commands;

namespace TaskManager.Application.Modules.Email.Sender;

public sealed class EmailSenderService(IOptions<EmailSenderOptions> options,
                                       ICodeStorage storage,
                                       ILogger<EmailSenderService> logger,
                                       IMailMessageFactory messageFactory) : IEmailSenderService
{
    public async Task SendAsync(ISendEmailMessageCommand message, CancellationToken cancellationToken)
        => await message.SendAsync(cancellationToken);

    public async Task SendVerificationCodeAsync(string to, CancellationToken cancellationToken)
    {
        var msg = new SendVerificationEmailMessageCommand(options, to, storage, logger , messageFactory);
        
        await SendAsync(msg, cancellationToken);
    }

    public async Task SendRecoveryCodeAsync(string to, CancellationToken cancellationToken)
    {
        var msg = new SendRecoveryCodeEmailMessageCommand(options, storage, to, messageFactory);

        await SendAsync(msg, cancellationToken);
    }
}
