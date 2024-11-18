namespace TaskManager.Application.User.Common.Security.Hashers;

public interface IPasswordHasher
{
    public string HashPassword(string password, string salt);
    public string GenerateSalt(int workFactor = 11);
    public bool Verify(string text, string hash);
}
