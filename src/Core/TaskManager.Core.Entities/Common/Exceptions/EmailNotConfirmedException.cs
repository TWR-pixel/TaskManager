namespace TaskManager.Core.Entities.Common.Exceptions;

public class EmailNotConfirmedException : Exception
{
    public EmailNotConfirmedException()
    {
    }

    public EmailNotConfirmedException(string? message) : base(message)
    {
    }

    public EmailNotConfirmedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
