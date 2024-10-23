namespace TaskManager.Core.Entities.Common.Exceptions;

public class EmailNotVerifiedException : Exception
{
    public EmailNotVerifiedException()
    {
    }

    public EmailNotVerifiedException(string? message) : base(message)
    {
    }

    public EmailNotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
