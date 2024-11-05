using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Application.Users.Exceptions;

public class EmailNotVerifiedException : NotVerifiedException
{

    public EmailNotVerifiedException(string? email) : base($"User with email '{email}' not verified")
    {
    }

    public EmailNotVerifiedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
