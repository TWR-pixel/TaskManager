namespace TaskManager.Application.Modules.Email.Sender.Commands;

public interface ISendEmailMessageCommand
{
    public Task SendAsync(CancellationToken cancellationToken);
}
