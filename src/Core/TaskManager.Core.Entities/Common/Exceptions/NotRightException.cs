namespace TaskManager.Domain.Entities.Common.Exceptions;

public class NotRightException : Exception
{
    public NotRightException(string? message) : base(message)
    {
    }

    public NotRightException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
