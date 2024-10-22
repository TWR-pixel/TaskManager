using TaskManager.Application.Modules.Email.Code.Storage;

namespace TaskManager.Application.Modules.Email.Verifier;

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
