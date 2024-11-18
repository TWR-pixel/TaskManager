using TaskManager.Domain.Entities.Users;

namespace TaskManager.Application.User.Common.Security.AccessToken;

public interface IAccessTokenFactory
{
    public AccessTokenResponse Create(UserEntity user);
}
