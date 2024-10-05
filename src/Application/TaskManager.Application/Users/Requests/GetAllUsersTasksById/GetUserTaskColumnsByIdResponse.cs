namespace TaskManager.Application.Users.Requests.GetAllUsersTasksById;

public sealed class GetUserTaskColumnsByIdResponse
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
