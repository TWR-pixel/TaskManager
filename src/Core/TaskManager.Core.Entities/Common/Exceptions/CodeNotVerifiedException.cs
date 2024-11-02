namespace TaskManager.Core.Entities.Common.Exceptions;

public class CodeNotVerifiedException : NotVerifiedException
{
    public CodeNotVerifiedException(string? code) : base($"Code with value '{code}' not verified")
    {
    }

    public CodeNotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
