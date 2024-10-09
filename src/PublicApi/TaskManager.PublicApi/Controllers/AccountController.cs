using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

public sealed class AccountController(IMediatorFacade mediator) : ApiControllerBase(mediator)
{
}
