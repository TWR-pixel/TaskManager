namespace TaskManager.Application.Common.Security.Authentication;

public interface IUserSignInManager
{
    public void Login(string refreshToken);
    public void Logout();
    public void CreateRefreshToken(string refreshToken);
}
