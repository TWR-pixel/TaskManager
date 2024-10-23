namespace TaskManager.Application.Modules.Email.Code.Storage;

public interface ICodeStorage
{
    public void Set(string code, string email);
    public bool TryGetValue(string code, out string? email);
    public void Remove(string code);
}
