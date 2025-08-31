namespace SchoolSync.API.Helpers;

public static class UploadHandler
{
    public static async Task<UploadResult> HandlePdfUpload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return new UploadResult { Success = false, ErrorMessage = "No file uploaded." };

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return new UploadResult { Success = false, ErrorMessage = "Only PDF files are allowed." };

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return new UploadResult
        {
            Success = true,
            FileData = ms.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
    }

    public static async Task<UploadResult> HandleImageUpload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return new UploadResult { Success = false, ErrorMessage = "No file uploaded." };

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (Array.IndexOf(allowedTypes, file.ContentType.ToLower()) == -1)
            return new UploadResult { Success = false, ErrorMessage = "Only JPEG, PNG, or GIF images are allowed." };

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return new UploadResult
        {
            Success = true,
            FileData = ms.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
    }

    public static async Task<UploadResult> HandleVideoUpload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return new UploadResult { Success = false, ErrorMessage = "No file uploaded." };

        var allowedTypes = new[] { "video/mp4", "video/avi", "video/mpeg", "video/quicktime" };
        if (Array.IndexOf(allowedTypes, file.ContentType.ToLower()) == -1)
            return new UploadResult { Success = false, ErrorMessage = "Only MP4, AVI, MPEG, or MOV videos are allowed." };

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return new UploadResult
        {
            Success = true,
            FileData = ms.ToArray(),
            FileName = file.FileName,
            ContentType = file.ContentType
        };
    }
}
public class UploadResult
{
    public bool Success { get; set; }
    public byte[] FileData { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string? ErrorMessage { get; set; }

}