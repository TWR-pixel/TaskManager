﻿using FluentValidation;
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
                                                         IRandomFileNameGenerator fileNameGenerator,
                                                         IValidator<UploadUserProfileImageCommand> commandValidator) : CommandHandlerBase<UploadUserProfileImageCommand, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(UploadUserProfileImageCommand command, CancellationToken cancellationToken)
    {
        await commandValidator.ValidateAndThrowAsync(command, cancellationToken);

        var userEntity = await UnitOfWork.Users.GetWithRoleByIdAsync(command.UserId, cancellationToken)
            ?? throw new UserNotFoundException(command.UserId);

        var randomFileName = fileNameGenerator.GenerateRandomFileName();
        var profileImageLink = command.ProfileImageLink + "/" + "profile-image?ImageName=" + randomFileName;

        userEntity.ProfileImageLink = profileImageLink;
        await SaveChangesAsync(cancellationToken);
        var fileExtension = Path.GetExtension(command.FormFile.FileName);

        if (string.IsNullOrWhiteSpace(fileExtension))
            throw new NotSupportedException(nameof(fileExtension));



        fileWriter.WriteToFromFormFile(randomFileName, command.FormFile);

        var response = userEntity.ToResponse();

        return response;
    }
}
