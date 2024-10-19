using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.Common.EmailSender;

public sealed record EmailSenderOptions
{
    [SetsRequiredMembers]
    public EmailSenderOptions(string from, string password, string smtpAddress, int port)
    {
        From = from;
        Password = password;
        SmtpAddress = smtpAddress;
        Port = port;
    }

    public required string From { get; set; }
    public required string SmtpAddress { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }

}
