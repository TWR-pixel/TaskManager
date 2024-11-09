namespace TaskManager.Domain.Entities.Users.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string email)
        : base($"User with email '{email}' not found.") { }

    public UserNotFoundException(int userId)
        : base($"User with id '{userId}' not found.") { }
}
