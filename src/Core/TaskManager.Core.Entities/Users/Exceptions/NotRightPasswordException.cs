using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Core.Entities.Users.Exceptions;

public class NotRightPasswordException : NotRightException
{
    public NotRightPasswordException(string currentPassword)
        : base($"")
    {

    }
}
