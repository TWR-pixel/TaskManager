using TaskManager.Application.Common.File;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record GetUserProfileImageQuery : QueryBase<FileStream>
{
    public required string ImageName { get; set; }
}

public sealed class GetUserProfileImageQueryHandler(IReadonlyUnitOfWork unitOfWork,
                                                    IFileReader fileReader) : QueryHandlerBase<GetUserProfileImageQuery, FileStream>(unitOfWork)
{
    public override async Task<FileStream> Handle(GetUserProfileImageQuery request, CancellationToken cancellationToken)
    {
        var image = fileReader.OpenRead(request.ImageName);

        return image;
    }
}
