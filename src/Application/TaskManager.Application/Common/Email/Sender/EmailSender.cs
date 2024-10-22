using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.Common.Email.Sender;

public sealed class EmailSender(IOptions<EmailSenderOptions> options, IMemoryCache cache, IUnitOfWork unitOfWork) : IEmailSender
{
    public EmailSenderOptions Options { get; init; } = options.Value;

    private readonly IMemoryCache _cache = cache;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly SmtpClient _smtpClient = options.Value.SmtpClient;

    public async Task SendMailAsync(MailMessage message, CancellationToken cancellationToken)
    {
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }

    public async Task SendVerificationCodeAsync(string To, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.SingleOrDefaultAsync(new ReadUserByEmailSpec(To), cancellationToken)
            ?? throw new UserNotFoundException(To);

        var verificationCode = RandomNumberGenerator.GetHexString(20, false);

        var options = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7), // 7 minutes alive
        };

        _cache.Set(verificationCode, user.Id.ToString(), options);

        var msg = new MailMessage(Options.From, To, "Email verification", $"Verification code <h2>{verificationCode}</h2>")
        {
            IsBodyHtml = true
        };

        await SendMailAsync(msg, cancellationToken);
    }
}
