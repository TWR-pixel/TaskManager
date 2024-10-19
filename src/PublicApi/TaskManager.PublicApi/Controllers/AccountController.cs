using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

public sealed class AccountController(IMediatorWrapper mediator) : ApiControllerBase(mediator)
{
}
