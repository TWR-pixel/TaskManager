namespace TaskManager.Application.Users.Requests.GetUsersTasksById;

public sealed class GetUserTasksByIdResponse
{
    public required string Title { get; set; }
    public required string Content { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

   // public required GetUserTaskColumnsByIdResponse TaskColumn { get; set; }
}
