using TaskManager.Application.Common.Security.SymmetricSecurityKeys;

namespace TaskManager.Application.Common.Security.Auth.Jwt.Options;

public class JwtAuthenticationOptions
{
    public string SecretKey { get; set; } = "CHANGE_ME";
    public int ExpiresTokenHours { get; set; } = 12; // 12 hours
    public int ExpiresTokenMinutes { get; set; } = 5;
    public string Issuer { get; set; } = "ISSUER_DEFAULT_CHANGE_ME";
    public string Audience { get; set; } = "AUDIENCE_DEFAULT_CHANGE_ME";
    public ISymmetricSecurityKeysGenerator SecurityKeysGenerator { get; set; } = new SymmetricSecurityKeysGenerator();

}
