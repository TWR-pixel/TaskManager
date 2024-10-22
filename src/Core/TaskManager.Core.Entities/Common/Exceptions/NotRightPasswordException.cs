namespace TaskManager.Core.Entities.Common.Exceptions;

public class NotRightPasswordException : Exception
{
    public NotRightPasswordException(string currentPassword)
        : base($"")
    {
        
    }
}
