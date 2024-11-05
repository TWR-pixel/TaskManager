using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Tasks;

namespace TaskManager.Application.TaskColumns;

public sealed record UserTaskColumnDto
{
    [SetsRequiredMembers]
    public UserTaskColumnDto(string title, string? description, IEnumerable<UserTaskDto>? tasksInColumn, int ownerId, int id)
    {
        Title = title;
        Description = description;
        TasksInColumn = tasksInColumn;
        OwnerId = ownerId;
        Id = id;
    }

    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<UserTaskDto>? TasksInColumn { get; set; }
    public required int OwnerId { get; set; }
}
