namespace TaskManager.Core.Entities.Common.Exceptions;

public class UserAlreadyVerifiedException : Exception
{
    public UserAlreadyVerifiedException()
    {
    }

    public UserAlreadyVerifiedException(string email)
        : base($"User with email '{email}' already confirmed")
    {
    }
}
