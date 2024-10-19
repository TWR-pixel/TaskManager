namespace TaskManager.Core.Entities.Common.Exceptions;

public class NotRightCodeException : Exception
{
    public NotRightCodeException()
    {
    }

    public NotRightCodeException(string? message) : base(message)
    {
    }

    public NotRightCodeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
