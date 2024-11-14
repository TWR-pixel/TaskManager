using FluentValidation;

namespace TaskManager.Application.User.Commands.Register.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
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
