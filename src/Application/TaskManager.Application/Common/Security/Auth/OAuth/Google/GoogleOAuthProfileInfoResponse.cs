using System.Text.Json.Serialization;

namespace TaskManager.Application.Common.Security.Auth.OAuth.Google;

public class GoogleOAuthProfileInfoResponse
{
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }
}
