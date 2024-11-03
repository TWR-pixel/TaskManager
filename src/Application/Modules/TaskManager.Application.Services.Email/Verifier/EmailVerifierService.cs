using System.Net.Http.Json;

namespace TaskManager.Application.Modules.Email.Verifier;

public sealed class EmailVerifierService(HttpClient client, string apiKey) : IEmailVerifierService
{
    public async Task<EmailVerifierServiceResponse> Verify(string email, CancellationToken cancellationToken)
    {
        var request = new EmailVerificationRequest
        {
            ApiKey = apiKey,
            EmailAddress = email
        };

        var response = await client.PostAsJsonAsync("https://verify.maileroo.net/check", request, cancellationToken);

        throw new NotImplementedException();
    }
}
