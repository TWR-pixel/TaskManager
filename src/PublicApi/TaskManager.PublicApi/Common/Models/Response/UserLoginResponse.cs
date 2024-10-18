using TaskManager.Application.Users.Requests.Identity.Authenticate;

namespace TaskManager.PublicApi.Common.Models.Response;

public sealed class UserLoginResponse
{
    public required string AccessTokenString { get; set; }

    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }

    public static implicit operator UserLoginResponse(AuthenticateUserResponse response) => new()
    {
        AccessTokenString = response.AccessToken,
        RoleId = response.RoleId,
        RoleName = response.RoleName,
        UserId = response.UserId,
        UserName = response.AccessToken,
    };
}
