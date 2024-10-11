namespace TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;

public interface IJwtRefreshTokenGenerator
{
    public string GenerateRefreshToken(int stringLength = 128);
}