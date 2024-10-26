using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Messages;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendVerificationEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        string to,
                                                        ICodeStorage storage,
                                                        ILogger<EmailSenderService> logger,
                                                        IMailMessageFactory messageFactory) : ISendEmailMessageCommand
{
    private readonly ILogger<EmailSenderService> _logger = logger;
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly ICodeStorage _storage = storage;
    private readonly string _to = to;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        //await _options.SmtpClient.VerifyAsync(_to, cancellationToken);

        using var msg = messageFactory.CreateVerification(_to);

        _logger.LogInformation("Message has started sending from {From} to {To}", _options.From, _to);

        await _options.SmtpClient.SendAsync(msg, cancellationToken);

        _logger.LogInformation("Message was sent successfully!");

    }
}
