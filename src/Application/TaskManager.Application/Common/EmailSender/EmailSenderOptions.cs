using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace TaskManager.Application.Common.EmailSender;

public sealed record EmailSenderOptions
{
    public required string From { get; set; }
    public required string SmtpAddress { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }
    public required SmtpClient SmtpClient { get; set; }

}
