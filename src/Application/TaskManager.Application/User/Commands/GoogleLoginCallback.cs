using Microsoft.AspNetCore.Identity;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record GoogleLoginCallback : CommandBase<string>
{
    public required string ReturnUrl { get; set; }
}

public sealed class GoogleLoginCallbackCommandHandler(IUnitOfWork unitOfWork, UserManager<UserEntity> userManager) : CommandHandlerBase<GoogleLoginCallback, string>(unitOfWork)
{
    public override Task<string> Handle(GoogleLoginCallback request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
