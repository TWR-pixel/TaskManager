namespace TaskManager.Application.Users.Requests.GetUsersTasksById;

public sealed class GetUserTaskColumnsByIdResponse
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
