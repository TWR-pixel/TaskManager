namespace TaskManager.Application.Tasks.Requests.UpdateAndSaveTaskRequest;

public sealed class UpdatePatchAndSaveTaskDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IsInProgress { get; set; }
}
