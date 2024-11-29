using Microsoft.AspNetCore.Http;
using TaskManager.Application.Common.File;
using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Commands;

public sealed record UploadUserProfileImageCommand : CommandBase<UserDto>
{
    public required int UserId { get; set; }
    public required IFormFile FormFile { get; set; }
    public required string ProfileImageLink { get; set; }
}

public sealed class UploadUserProfileImageCommandHandler(IUnitOfWork unitOfWork,
                                                         IFileWriter fileWriter,
                                                         IRandomFileNameGenerator fileNameGenerator) : CommandHandlerBase<UploadUserProfileImageCommand, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(UploadUserProfileImageCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetWithRoleByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var randomFileName = fileNameGenerator.GenerateRandomFileName();
        var profileImageLink = request.ProfileImageLink + "/" + "profile-image?ImageName=" + randomFileName;

        userEntity.ProfileImageLink = profileImageLink;
        await SaveChangesAsync(cancellationToken);

        fileWriter.WriteToFromFormFile(randomFileName, request.FormFile);

        var response = userEntity.ToResponse();

        return response;
    }
}
