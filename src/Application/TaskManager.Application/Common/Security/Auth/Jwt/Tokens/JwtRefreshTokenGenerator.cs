using System.Security.Cryptography;

namespace TaskManager.Application.Common.Security.Auth.Jwt.Tokens;

public sealed class JwtRefreshTokenGenerator : IJwtRefreshTokenGenerator
{
    public string GenerateRefreshToken(int stringLength = 128)
    {
        var value = RandomNumberGenerator.GetHexString(stringLength);

        return value;
    }
}
