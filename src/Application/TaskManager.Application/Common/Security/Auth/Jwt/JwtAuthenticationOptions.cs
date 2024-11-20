using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.Application.Common.Security.Auth.Jwt;

public class JwtAuthenticationOptions
{
    public string SecretKey { get; set; } = "DEFAULT_KEY_CHANGE_ME";
    public int ExpiresTokenHours { get; set; } = 12; // 12 hours
    public int ExpiresTokenMinutes { get; set; } = 5;
    public string Issuer { get; set; } = "DEFAULT_ISSUER";
    public string Audience { get; set; } = "DEFAULT_AUDIENCE";
}
