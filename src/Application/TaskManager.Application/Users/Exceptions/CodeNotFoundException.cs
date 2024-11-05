using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Application.Users.Exceptions;

public class CodeNotFoundException : NotFoundException
{
    public CodeNotFoundException(string? message) : base(message)
    {
    }

    public CodeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
