using Microsoft.AspNetCore.Mvc;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

public abstract class CrudApiControllerBase(IMediatorFacade mediator) : ControllerBase
{
    private readonly IMediatorFacade _mediator = mediator;

    protected IMediatorFacade Mediator => _mediator;
}
