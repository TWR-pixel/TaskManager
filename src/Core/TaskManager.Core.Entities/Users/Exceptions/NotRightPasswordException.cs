using TaskManager.Domain.Entities.Common.Exceptions;

namespace TaskManager.Domain.Entities.Users.Exceptions;

public class NotRightPasswordException : NotRightException
{
    public NotRightPasswordException(string currentPassword)
        : base($"")
    {

    }
}
