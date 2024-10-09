namespace TaskManager.PublicApi.Controllers;

public class RegisterUserModelResponse
{
    public required string AccessTokenString { get; set; }

    public required int UserId { get; set; }
    public required string Username { get; set; }

    public required int RoleId { get; set; }
    public required string RoleName { get; set; }

}