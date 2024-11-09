namespace TaskManager.Domain.Entities.Common.Exceptions;

public class NotRightCodeException : NotRightException
{
    public NotRightCodeException(string? message) : base(message)
    {
    }

    public NotRightCodeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
