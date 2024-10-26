using System.Net.Mail;

namespace TaskManager.Application.Modules.Email.Options;

public sealed record EmailSenderOptions()
{
    public required string From { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }
    public required SmtpClient SmtpClient { get; set; }
}
