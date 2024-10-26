namespace TaskManager.Application.Modules.Email.Code.Verifier;

public interface ICodeVerifier
{
    public bool Verify(string code, out string email);
}