namespace TaskManager.PublicApi.Common.Authentication.Services;

public interface IUserAuthService
{
    public void CreateUser(string refreshToken);
}
