namespace TaskManager.Core.Entities.TaskColumns;

public class TaskColumnNotFoundException(int columnId) : Exception($"Column with id '{columnId}' not found")
{
}
