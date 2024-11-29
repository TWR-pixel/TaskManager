using FluentValidation;
using TaskManager.Application.User.Commands;

namespace TaskManager.Infrastructure.Validator.User;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .Matches(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5);
    }
}
