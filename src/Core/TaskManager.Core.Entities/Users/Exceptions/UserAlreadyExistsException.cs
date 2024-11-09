namespace TaskManager.Domain.Entities.Users.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string email) : base($"User with email '{email}' alread exists")
    {
    }

}
