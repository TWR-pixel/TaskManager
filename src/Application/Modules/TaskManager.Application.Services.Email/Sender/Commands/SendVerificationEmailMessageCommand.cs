﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Application.Modules.Email.Messages;
using TaskManager.Application.Modules.Email.Options;

namespace TaskManager.Application.Modules.Email.Sender.Commands;

public sealed class SendVerificationEmailMessageCommand(IOptions<EmailSenderOptions> Options,
                                                        string to,
                                                        ILogger<EmailSenderService> logger,
                                                        IMailMessageFactory messageFactory) : ISendEmailMessageCommand
{
    private readonly ILogger<EmailSenderService> _logger = logger;
    private readonly EmailSenderOptions _options = Options.Value;
    private readonly string _to = to;
    private readonly SmtpClient _client = Options.Value.SmtpClient;

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        using var msg = messageFactory.CreateVerification(_to);

        _logger.LogInformation("Message has started sending from {From} to {To}", _options.From, _to);

        _client.Connect(_options.Host, _options.Port, true, cancellationToken);
        _client.Authenticate(_options.From, _options.Password, cancellationToken);

        await _options.SmtpClient.SendAsync(msg, cancellationToken);

        _logger.LogInformation("Message was sent successfully!");

        await _client.DisconnectAsync(true, cancellationToken);
    }
}
