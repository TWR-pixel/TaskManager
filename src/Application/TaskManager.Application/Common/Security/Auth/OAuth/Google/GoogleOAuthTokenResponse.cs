using System.Text.Json.Serialization;

namespace TaskManager.Application.Common.Security.Auth.OAuth.Google;

public sealed class GoogleOAuthTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } = string.Empty;
}
