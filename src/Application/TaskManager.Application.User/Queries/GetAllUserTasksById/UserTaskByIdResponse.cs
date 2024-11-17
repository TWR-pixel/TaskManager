namespace TaskManager.Application.User.Queries.GetAllUserTasksById;

public sealed record UserTaskByIdResponse(string Title,
                                          string? Description,
                                          bool IsCompleted,
                                          bool IsInProgress,
                                          DateTime CreatedAt,
                                          DateOnly? CompletedAt,
                                          int Id,
                                          int ColumnId)
{

}