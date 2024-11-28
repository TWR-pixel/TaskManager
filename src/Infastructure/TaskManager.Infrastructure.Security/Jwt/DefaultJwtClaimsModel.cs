namespace TaskManager.Infrastructure.Security.Jwt;

public sealed class DefaultJwtClaimsModel
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required int RoleId { get; set; }
    public required string RoleName { get; set; }
}
