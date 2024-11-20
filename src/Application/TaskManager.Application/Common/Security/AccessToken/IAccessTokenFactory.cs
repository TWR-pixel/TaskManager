using TaskManager.Domain.Entities.Users;

namespace TaskManager.Application.Common.Security.AccessToken;

public interface IAccessTokenFactory
{
    public AccessTokenResponse Create(UserEntity user);
}
