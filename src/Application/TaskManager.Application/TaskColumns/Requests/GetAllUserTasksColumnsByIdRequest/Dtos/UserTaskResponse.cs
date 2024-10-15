using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskResponse
{
    [SetsRequiredMembers]
    public UserTaskResponse(string name, string content)
    {
        Name = name;
        Content = content;
    }

    public required string Name { get; set; }
    public required string Content { get; set; }
}