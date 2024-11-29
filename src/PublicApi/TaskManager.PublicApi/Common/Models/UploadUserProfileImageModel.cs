namespace TaskManager.PublicApi.Common.Models;

public class UploadUserProfileImageModel
{
    public required int UserId { get; set; }

    public required IFormFile UserProfileImage { get; set; }
}
