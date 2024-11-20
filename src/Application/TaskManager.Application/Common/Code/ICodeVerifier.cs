namespace TaskManager.Application.Common.Code;

public interface ICodeVerifier
{
    public bool Verify(string code, string email);
}