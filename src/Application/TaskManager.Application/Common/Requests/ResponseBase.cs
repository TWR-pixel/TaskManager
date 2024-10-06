namespace TaskManager.Application.Common.Requests;

public abstract class ResponseBase
{
    public string ResponseStatus { get; set; } = "Success";
}
