namespace TaskManager.Application.User.Common.Email.Code.Verifier;

public interface ICodeVerifier
{
    public bool Verify(string code, out string email);
}