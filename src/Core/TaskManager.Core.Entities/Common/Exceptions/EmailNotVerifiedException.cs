namespace TaskManager.Domain.Entities.Common.Exceptions;

public class EmailNotVerifiedException : NotVerifiedException
{

    public EmailNotVerifiedException(string? email) : base($"User with email '{email}' not verified")
    {
    }

    public EmailNotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
