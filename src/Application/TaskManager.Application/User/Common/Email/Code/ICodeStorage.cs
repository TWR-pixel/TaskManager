namespace TaskManager.Application.User.Common.Email.Code;

public interface ICodeStorage
{
    public void Create(string code, string email);
    public string? GetEmail(string code);
    public void Remove(string code);
}
