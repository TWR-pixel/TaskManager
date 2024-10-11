namespace TaskManager.PublicApi.Common.Authentication;

public interface IUserSignInManager
{
    public void Login(string refreshToken, HttpContext context);
    public void Logout(HttpContext context);
    public void CreateRefreshToken(string refreshToken, HttpContext context);
}
