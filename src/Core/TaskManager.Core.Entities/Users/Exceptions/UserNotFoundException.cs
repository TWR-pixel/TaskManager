using TaskManager.Domain.Entities.Common.Exceptions;

namespace TaskManager.Domain.Entities.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{

    public UserNotFoundException(string email)
        : base($"User with email '{email}' not found.") { }

    public UserNotFoundException(int userId)
        : base($"User with id '{userId}' not found.") { }
}
