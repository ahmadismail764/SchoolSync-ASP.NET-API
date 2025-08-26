using SchoolSync.Domain.Entities;
namespace SchoolSync.API.Helpers;

public class UploadHandler
{
    public string Upload(IFormFile file)
    {
        List<string> validExtensions = [".jpg", ".png", ".gif"];
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!validExtensions.Contains(extension))
            return $"Extension is not valid ({string.Join(", ", validExtensions)})";
        long size = file.Length;
        if (size > (5 * 1024 * 1024)) // 5MB
            return "File size is too large. Maximum allowed size is 5MB.";
        string fileName = Guid.NewGuid().ToString() + extension;

        return "uploaded";
        // extension
        // file size
        // name changing
    }
}