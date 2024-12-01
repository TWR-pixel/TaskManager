namespace TaskManager.Domain.Entities.Common.Exceptions;

public class GoogleOAuthRegisteredException : Exception
{
    public GoogleOAuthRegisteredException(string email) : base($"User with email '{email}' already registered by Google.")
    {

    }
}
