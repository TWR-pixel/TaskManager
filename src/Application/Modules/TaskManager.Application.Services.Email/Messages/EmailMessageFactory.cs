using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Cryptography;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Options;
using TaskManager.Application.Modules.Email.Sender;

namespace TaskManager.Application.Modules.Email.Messages;

public sealed class EmailMessageFactory(IOptions<EmailSenderOptions> Options,
                                        ICodeStorage codeStorage,
                                        ILogger<EmailSenderService> logger) : IMailMessageFactory
{
    private readonly EmailSenderOptions options = Options.Value;

    public MimeMessage Create(string to, string subject, string body)
    {
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("Administration task-manager", options.From));
        msg.To.Add(new MailboxAddress(to, to));
        msg.Subject = subject;
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = body
        };

        logger.LogInformation("Message was successfully created");

        return msg;
    }

    public MimeMessage CreateRecoveryPassword(string to)
    {
        var code = RandomNumberGenerator.GetHexString(20);

        codeStorage.Set(code, to);

        var msg = Create(to, "Password reset requested", $"<h1>{code}k</h1>");

        return msg;
    }

    public MimeMessage CreateVerification(string to)
    {
        logger.LogInformation("Verification code generation");
        var code = RandomNumberGenerator.GetHexString(20);
        logger.LogInformation("Generated code {VerificationCode}", code);


        codeStorage.Set(code, to);
        logger.LogInformation("Code has been saved to the repository with key {Code} and with email {To}", code, to);


        logger.LogInformation("Creating a verification message");
        var msg = Create(to, "Verification email", $"<h1>{code}</h1>");

        return msg;
    }
}
