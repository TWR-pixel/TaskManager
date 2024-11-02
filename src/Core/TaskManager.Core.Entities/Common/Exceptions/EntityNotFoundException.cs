namespace TaskManager.Core.Entities.Common.Exceptions;

public sealed class EntityNotFoundException : NotFoundException
{
    public EntityNotFoundException(string? message) : base(message) { }

    public EntityNotFoundException(string name, int id)
        : base($"{name} with ID '{id}' was not found.")
    {
    }


    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}