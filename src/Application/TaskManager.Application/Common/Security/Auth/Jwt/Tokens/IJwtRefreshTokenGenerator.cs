namespace TaskManager.Application.Common.Security.Auth.Jwt.Tokens;

public interface IJwtRefreshTokenGenerator
{
    public string GenerateRefreshToken(int stringLength = 128);
}