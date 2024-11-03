using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.Common.Security.Auth.AccessToken;

public interface IAccessTokenFactory
{
    public AccessTokenResponse Create(UserEntity user);
}
