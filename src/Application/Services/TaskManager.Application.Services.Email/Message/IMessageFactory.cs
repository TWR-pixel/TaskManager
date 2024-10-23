using System.Net.Mail;

namespace TaskManager.Application.Modules.Email.Message;

public interface IMessageFactory
{
    public MailMessage Create(string to);
}
