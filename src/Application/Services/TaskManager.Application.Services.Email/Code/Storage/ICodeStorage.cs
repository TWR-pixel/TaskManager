namespace TaskManager.Application.Modules.Email.Code.Storage;

public interface ICodeStorage
{
    public void Set(string key, string value);
    public bool TryGetValue(string key, out string? value);
    public void Remove(string key);
}
