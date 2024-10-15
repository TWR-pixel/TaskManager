namespace TaskManager.Core.Entities.Common.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string? message) : base(message)
    {
    }

    public UserAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
