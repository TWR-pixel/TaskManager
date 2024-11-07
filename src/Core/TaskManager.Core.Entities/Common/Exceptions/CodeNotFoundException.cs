namespace TaskManager.Core.Entities.Common.Exceptions;

public class CodeNotFoundException : NotFoundException
{
    public CodeNotFoundException(string? message) : base(message)
    {
    }

    public CodeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
