using TaskManager.Core.Entities.Users;

namespace TaskManager.Application.User.Common.AccessToken;

public interface IAccessTokenFactory
{
    public AccessTokenResponse Create(UserEntity user);
}
