namespace TaskManager.Core.Entities.Common.Exceptions;

public class CodeNotVerifiedException : Exception
{
    public CodeNotVerifiedException()
    {
    }

    public CodeNotVerifiedException(string? message) : base(message)
    {
    }
}
