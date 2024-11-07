namespace TaskManager.Application.Common.Requests;

public abstract record ResponseBase
{
    public string Status { get; set; } = "Success";
}
