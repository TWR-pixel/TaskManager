using Microsoft.AspNetCore.Mvc;

namespace TaskManager.PublicApi.Controllers;

public abstract class ApiControllerBase(IMediatorWrapper mediator) : ControllerBase
{
    private readonly IMediatorWrapper _mediator = mediator;

    protected IMediatorWrapper Mediator => _mediator;
}
