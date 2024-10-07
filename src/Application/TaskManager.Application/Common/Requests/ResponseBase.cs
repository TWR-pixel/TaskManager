namespace TaskManager.Application.Common.Requests;

public abstract class ResponseBase
{
    public string Status { get; set; } = "Success";
}
