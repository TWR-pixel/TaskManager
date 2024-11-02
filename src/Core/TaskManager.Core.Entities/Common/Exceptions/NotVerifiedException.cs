namespace TaskManager.Core.Entities.Common.Exceptions;

public class NotVerifiedException : Exception
{
    public NotVerifiedException(string? message) : base(message)
    {
    }

    public NotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
