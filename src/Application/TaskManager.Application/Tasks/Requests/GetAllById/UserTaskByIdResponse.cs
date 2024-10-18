namespace TaskManager.Application.Tasks.Requests.GetAllById;

public sealed record UserTaskByIdResponse(string Title,
                                          string Content,
                                          bool IsCompleted,
                                          bool IsInProgress,
                                          DateTime CreatedAt,
                                          DateOnly? DoTo,
                                          int Id)
{

}
