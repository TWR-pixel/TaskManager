namespace TaskManager.Core.Entities.Common.Exceptions;

public class CodeNotVerifiedException : Exception
{
    public CodeNotVerifiedException()
    {
    }

    public CodeNotVerifiedException(string? message) : base(message)
    {
    }

    public CodeNotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
