using Microsoft.AspNetCore.Mvc;
using TaskManager.PublicApi.Common.Wrappers.Mediator;

namespace TaskManager.PublicApi.Controllers;

public abstract class ApiControllerBase(IMediatorWrapper mediator) : ControllerBase
{
    private readonly IMediatorWrapper _mediator = mediator;

    protected IMediatorWrapper Mediator => _mediator;
}
