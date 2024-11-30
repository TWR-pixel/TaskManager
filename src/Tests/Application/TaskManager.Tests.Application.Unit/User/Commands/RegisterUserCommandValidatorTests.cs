using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Application.Unit.User.Commands;

public class RegisterUserCommandValidatorTests : TestInitializer
{
    private readonly RegisterUserRequest DefaultRequest = new("UserName", "1@1.mail.ru", "123456");

    [Fact]
    public void ValidEmail_ShouldNotThrowsValidationException()
    {
        var validator = new RegisterUserValidator();

        AssertWrapper.DoesntThrow<ValidationException>(() => validator.ValidateAndThrowAsync(DefaultRequest));
    }

    [Fact]
    public void InvalidEmail_ShouldThrowsValidationException()
    {
        var validator = new RegisterUserValidator();
        var request = new RegisterUserRequest("UserName", "1@1", "123456");

        Assert.Throws<ValidationException>(() => validator.ValidateAndThrow(request));
    }
}
