namespace TaskManager.Core.Entities.Common.Exceptions;

public class NotRightPasswordException : NotRightException
{
    public NotRightPasswordException(string currentPassword)
        : base($"")
    {
        
    }
}
