namespace TaskManager.Application.Modules.Email.Verifier;

public interface ICodeVerifier
{
    public bool Verify(string code, out string email);
}