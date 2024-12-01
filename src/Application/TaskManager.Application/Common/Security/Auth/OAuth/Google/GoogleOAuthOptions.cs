namespace TaskManager.Application.Common.Security.Auth.OAuth.Google;

public sealed class GoogleOAuthOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string ServerRedirectUri { get; set; }
    public required string ClientRedirectUri { get; set; }
    public required string GrantType { get; set; }
}
