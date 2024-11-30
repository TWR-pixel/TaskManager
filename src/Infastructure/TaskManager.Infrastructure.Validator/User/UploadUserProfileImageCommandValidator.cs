using FluentValidation;
using TaskManager.Application.User.Commands;

namespace TaskManager.Infrastructure.Validator.User;

public sealed class UploadUserProfileImageCommandValidator : AbstractValidator<UploadUserProfileImageCommand>
{
    public UploadUserProfileImageCommandValidator()
    {
        RuleFor(u => u.FormFile)
            .NotEmpty()
            .NotNull();

        RuleFor(u => u.FormFile.Length)
            .LessThan(2000_000); // 2 megabytes
    }
}
