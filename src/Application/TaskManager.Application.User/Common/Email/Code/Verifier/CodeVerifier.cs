using TaskManager.Application.User.Common.Email.Code.Storage;

namespace TaskManager.Application.User.Common.Email.Code.Verifier;

public sealed class CodeVerifier(ICodeStorage storage) : ICodeVerifier
{
    private readonly ICodeStorage _storage = storage;

    public bool Verify(string code, out string email)
    {
        var isFound = _storage.TryGetValue(code, out email!);

        if (!isFound)
            return false;

        _storage.Remove(code);

        return true;
    }
}
