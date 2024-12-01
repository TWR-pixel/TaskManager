namespace TaskManager.Application.Common.Security.Auth.OAuth.Google;

public sealed class GoogleOAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string RedirectUri { get; set; }
    public required string GrantType { get; set; }
}
