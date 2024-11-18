namespace TaskManager.Application.User.Common.Security.Auth.Jwt.Claims;

public sealed class DefaultJwtClaims
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required int RoleId { get; set; }
    public required string RoleName { get; set; }
}
