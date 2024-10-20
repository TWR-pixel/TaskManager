using System.Net.Mail;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Common.Services.EmailSender;

public interface IEmailSender
{
    public EmailSenderOptions Options { get; init; }
    public Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default);
    public Task SendVerificationCodeAsync(string To, UserEntity user, CancellationToken cancellationToken = default);
}
