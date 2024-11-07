using System.Text.Json.Serialization;

namespace TaskManager.Application.User.Common.Email.Verifier;

public sealed class EmailVerificationRequest
{
    [JsonPropertyName("api_key")]
    public required string ApiKey { get; set; }

    [JsonPropertyName("email_address")]
    public required string EmailAddress { get; set; }

}
