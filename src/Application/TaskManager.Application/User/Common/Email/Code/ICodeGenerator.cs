namespace TaskManager.Application.User.Common.Email.Code;

public interface ICodeGenerator<in TResult> where TResult : class
{
    public string GenerateCode(int length);
}
