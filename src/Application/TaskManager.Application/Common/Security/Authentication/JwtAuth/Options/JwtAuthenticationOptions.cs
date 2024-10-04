namespace TaskManager.Application.Common.Security.Authentication.JwtAuth.Options;

public class JwtAuthenticationOptions
{
    public string SecurityKey { get; set; } = "CHANGE_ME";
    public int ExpiresTokenHours { get; set; } = 12; // 12 hours
    public string Issuer { get; set; } = "ISSUER_DEFAULT_CHANGE_ME";
    public string Audience { get; set; } = "AUDIENCE_DEFAULT_CHANGE_ME";
}
