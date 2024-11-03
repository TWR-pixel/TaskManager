using System.Text.Json.Serialization;

namespace TaskManager.Application.Modules.Email.Verifier;

public sealed class EmailVerificationRequest
{
    [JsonPropertyName("api_key")]
    public required string ApiKey { get; set; }

    [JsonPropertyName("email_address")]
    public required string EmailAddress { get; set; }

}
