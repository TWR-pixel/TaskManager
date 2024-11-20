namespace TaskManager.Application.Common.Code;

public interface ICodeStorage
{
    public void Add(string code, string email);
    public string? GetEmail(string code);
    public void Remove(string code);
}
