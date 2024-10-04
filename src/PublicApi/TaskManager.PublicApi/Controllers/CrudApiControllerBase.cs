using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Requests;
using TaskManager.PublicApi.Common;

namespace TaskManager.PublicApi.Controllers;

public abstract class CrudApiControllerBase : ControllerBase
{
    private readonly IMediatorFacade _mediator;

    protected IMediatorFacade Mediator => _mediator;

    protected CrudApiControllerBase(IMediatorFacade mediator)
    {
        _mediator = mediator;
    }

}
