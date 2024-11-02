using TaskManager.Application.Common.Security.Auth.AccessToken;

namespace TaskManager.PublicApi.Common.Models.Response;

public sealed class UserLoginResponse
{
    public required string AccessTokenString { get; set; }

    public required int UserId { get; set; }
    public required string Username { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }

    public static implicit operator UserLoginResponse(AccessTokenResponse response) => new()
    {
        AccessTokenString = response.AccessToken,
        RoleId = response.RoleId,
        RoleName = response.RoleName,
        UserId = response.UserId,
        Username = response.Username,
    };
}
