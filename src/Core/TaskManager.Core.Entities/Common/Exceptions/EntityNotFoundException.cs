namespace TaskManager.Core.Entities.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"Entity '{entityName}' with ID '{entityId}' was not found.")
    {
    }

    public EntityNotFoundException(string entity, string name)
        : base($"Entity '{entity}' with parameter '{name}' was not found")
    {

    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}