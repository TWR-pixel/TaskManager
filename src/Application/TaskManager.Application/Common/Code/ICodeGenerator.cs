namespace TaskManager.Application.Common.Code;

public interface ICodeGenerator<in TResult> where TResult : class
{
    public string GenerateCode(int length);
}
