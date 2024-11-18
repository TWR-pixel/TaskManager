namespace TaskManager.Application.User.Common.Email.Code;

public interface ICodeGenerator<in T> where T : class
{
    public string GenerateCode(int length);
}
