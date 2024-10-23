using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Cryptography;
using TaskManager.Application.Modules.Email.Code.Storage;
using TaskManager.Application.Modules.Email.Sender.Options;

namespace TaskManager.Application.Modules.Email.Message;

public sealed class RecoveryPasswordMessageFactory(ICodeStorage codeStorage, IOptions<EmailSenderOptions> options) : IRecoveryPasswordMessageFactory
{
    private readonly ICodeStorage _codeStorage = codeStorage;
    private readonly EmailSenderOptions _options = options.Value;

    public MailMessage Create(string to)
    {
        var code = RandomNumberGenerator.GetHexString(20);

        _codeStorage.Set(code, to);

        var msg = new MailMessage(_options.From, to, "Recovery code", code);
        
        return msg;
    }
}
