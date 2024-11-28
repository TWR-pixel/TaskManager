using TaskManager.Application.Common.Code;

namespace TaskManager.Infrastructure.Code;

public sealed class CodeVerifier(ICodeStorage storage) : ICodeVerifier
{
    private readonly ICodeStorage _storage = storage;

    public bool Verify(string code, string email)
    {
        var emailFromStorage = _storage.GetEmail(code);

        if (string.IsNullOrWhiteSpace(emailFromStorage))
            return false;

        return true;
    }
}
