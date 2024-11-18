namespace TaskManager.Application.User.Common.Email.Code;

public interface ICodeVerifier
{
    public bool Verify(string code, string email);
}