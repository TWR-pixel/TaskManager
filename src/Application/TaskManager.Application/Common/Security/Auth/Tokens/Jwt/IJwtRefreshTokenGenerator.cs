namespace TaskManager.Application.Common.Security.Auth.Tokens.Jwt;

public interface IJwtRefreshTokenGenerator
{
    public string GenerateRefreshToken(int stringLength = 128);
}