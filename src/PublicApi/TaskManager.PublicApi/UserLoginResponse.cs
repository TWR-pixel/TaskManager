namespace TaskManager.PublicApi;

public sealed class UserLoginResponse
{
    public required string AccessTokenString { get; set; }

    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }
}
