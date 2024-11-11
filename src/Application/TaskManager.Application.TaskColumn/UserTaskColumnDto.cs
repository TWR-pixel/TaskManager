using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.UserTask;

namespace TaskManager.Application.TaskColumn;

public sealed record UserTaskColumnDto
{
    [SetsRequiredMembers]
    public UserTaskColumnDto(string title, string? description, int ordering, IEnumerable<UserTaskDto>? tasksInColumn, int ownerId, int id)
    {
        Title = title;
        Description = description;
        TasksInColumn = tasksInColumn;
        OwnerId = ownerId;
        Id = id;
        Ordering = ordering;
    }

    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required int Ordering { get; set; }
    public IEnumerable<UserTaskDto>? TasksInColumn { get; set; }
    public required int OwnerId { get; set; }
}
