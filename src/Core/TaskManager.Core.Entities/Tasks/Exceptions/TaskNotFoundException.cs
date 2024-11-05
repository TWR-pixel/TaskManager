using TaskManager.Core.Entities.Common.Exceptions;

namespace TaskManager.Core.Entities.Tasks.Exceptions;

public sealed class TaskNotFoundException : NotFoundException
{
    public TaskNotFoundException(int taskId)
        : base($"Task with id '{taskId}' not found")
    {
    }

    public TaskNotFoundException(int taskId, Exception? innerException)
        : base($"Task with id '{taskId}' not found", innerException)
    {
    }
}
