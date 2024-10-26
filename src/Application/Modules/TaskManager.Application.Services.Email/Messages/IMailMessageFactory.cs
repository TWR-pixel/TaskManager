using MimeKit;

namespace TaskManager.Application.Modules.Email.Messages;

public interface IMailMessageFactory
{
    public MimeMessage Create(string to, string subject, string body);
    public MimeMessage CreateVerification(string to);
    public MimeMessage CreateRecoveryPassword(string to);
}
