namespace TaskManager.Core.Entities.Users.Exceptions;

public class UserAlreadyVerifiedException : Exception
{
    public UserAlreadyVerifiedException()
    {
    }

    public UserAlreadyVerifiedException(string email)
        : base($"User with email '{email}' already verified")
    {
    }
}
