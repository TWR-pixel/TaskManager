using TaskManager.Application.Users.Requests.Identity.Register;

namespace TaskManager.PublicApi.Common.Models.Response;

public class RegisterUserModelResponse
{
    public required string AccessTokenString { get; set; }

    public required int UserId { get; set; }
    public required string Username { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }


    public static implicit operator RegisterUserModelResponse(RegisterUserResponse response) => new()
    {
        AccessTokenString = response.AccessTokenString,
        RoleId = response.RoleId,
        RoleName = response.RoleName,
        UserId = response.UserId,
        Username = response.Username,
    };
}