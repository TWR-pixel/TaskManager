using System.Security.Cryptography;

namespace TaskManager.Application.Common.Security.Authentication.JwtAuth.JwtTokens;

public sealed class JwtRefreshTokenGenerator : IJwtRefreshTokenGenerator
{
    public string GenerateRefreshToken()
    {
        var value = RandomNumberGenerator.GetHexString(128);

        return value;
    }
}
