using MailerooClient.Email.Verification.Requests.Check;
using MailerooClient.Email.Verification;
using TaskManager.Application.Common.Email;

namespace TaskManager.Persistence.Email;

public sealed class EmailExistingChecker(IHttpClientFactory httpClientFactory) : IEmailExistingChecker
{
    public async Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateClient("Maileroo");
        var checkEmail = new CheckRequest(email);
        var client = new MailerooApiClient(new MailerooClientOptions("123"), httpClient); // test this
        var checkResponse = await client.SendRequestAsync(checkEmail, cancellationToken);

        if (checkResponse.Data is null)
            throw new HttpRequestException(nameof(checkResponse));

        return checkResponse.Data.MxFound;
    }
}
