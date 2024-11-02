namespace TaskManager.Application.Common.Security.Auth.AccessToken;

public sealed record AccessTokenResponse(string AccessToken, int UserId, string Username, int RoleId, string RoleName) : ResponseBase;
