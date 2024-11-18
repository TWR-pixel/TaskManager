namespace TaskManager.Application.User.Common.Email;

public interface IEmailExistingChecker
{
    public Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default);
}
