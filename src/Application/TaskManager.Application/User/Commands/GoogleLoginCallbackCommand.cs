using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Application.Common.Security.AccessToken;
using TaskManager.Application.Common.Security.Auth.OAuth.Google;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;

namespace TaskManager.Application.User.Commands;

public sealed record GoogleLoginCallbackCommand : CommandBase<AccessTokenResponse>
{
    public required string Code { get; set; }
}

public sealed class GoogleLoginCallbackCommandHandler(IUnitOfWork unitOfWork,
                                                      IOptions<GoogleOAuthOptions> googleOptions,
                                                      UserManager<UserEntity> userManager,
                                                      IAccessTokenFactory tokenFactory) : CommandHandlerBase<GoogleLoginCallbackCommand, AccessTokenResponse>(unitOfWork)
{
    public override async Task<AccessTokenResponse> Handle(GoogleLoginCallbackCommand command, CancellationToken cancellationToken)
    {
        var value = googleOptions.Value;

        var client = new HttpClient();
        var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(value.GrantType), "grant_type");
        formData.Add(new StringContent(value.ClientId), "client_id");
        formData.Add(new StringContent(value.ClientSecret), "client_secret");
        formData.Add(new StringContent(value.ServerRedirectUri), "redirect_uri");
        formData.Add(new StringContent(command.Code), "code");

        Console.WriteLine(await formData.ReadAsStringAsync(cancellationToken));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://accounts.google.com/o/oauth2/token")
        {
            Content = formData
        };

        var userEmailResponse = await client.SendAsync(httpRequest, cancellationToken);
        Console.WriteLine(await userEmailResponse.Content.ReadAsStringAsync(cancellationToken));

        var deserializedData = await JsonSerializer.DeserializeAsync<GoogleOAuthTokenResponse>(userEmailResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
            ?? throw new JsonException();

        var jwtHandler = new JwtSecurityTokenHandler();
        var decodedJwtToken = jwtHandler.ReadJwtToken(deserializedData.IdToken);

        var email = decodedJwtToken.Payload["email"] as string ?? throw new HttpRequestException();
        var userFromDatabase = await UnitOfWork.Users.GetByEmailAsync(email, cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Get, "https://openidconnect.googleapis.com/v1/userinfo");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedData.AccessToken);

        var resp = await client.SendAsync(request, cancellationToken);
        var profileInfo = await JsonSerializer.DeserializeAsync<GoogleOAuthProfileInfoResponse>(resp.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken)
                          ?? throw new JsonException();

        if (userFromDatabase is not null)
        {
            userFromDatabase.UserName = profileInfo.Name;
            userFromDatabase.Email = email;
            userFromDatabase.ProfileImageLink = profileInfo.Picture;
            userFromDatabase.EmailConfirmed = profileInfo.EmailVerified;

            await SaveChangesAsync(cancellationToken);

            return tokenFactory.Create(userFromDatabase);
        }

        var role = await UnitOfWork.Roles.GetByNameAsync(RoleConstants.User, cancellationToken)
            ?? throw new RoleNotFoundException(RoleConstants.User);

        var userEntity = new UserEntity(role, email, profileInfo.Name, profileInfo.Picture, profileInfo.EmailVerified)
        {
            AuthenticationScheme = GoogleOAuthDefaults.AuthenticationScheme
        };

        await userManager.CreateAsync(userEntity);
        await SaveChangesAsync(cancellationToken);

        var defaultColumns = new List<UserTaskColumnEntity>()
        {
            new(userEntity, "Нужно сделать"),
            new(userEntity,"В процессе"),
            new(userEntity, "Завершенные"),
        };

        await UnitOfWork.UserTaskColumns.AddRangeAsync(defaultColumns, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var accessToken = tokenFactory.Create(userEntity);

        return accessToken;
    }
}
