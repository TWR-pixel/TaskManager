using TaskManager.Domain.Entities.Common.Exceptions;

namespace TaskManager.Domain.Entities.Tasks.Exceptions;

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

    public TaskNotFoundException(string title)
        : base($"Task with title '{title}' not found")
    {

    }
}
