using SchoolSync.Domain.Entities;
using System.IO;

namespace SchoolSync.API.Helpers;

public class UploadResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public long FileSize { get; set; }
    public MaterialType? MaterialType { get; set; }
}

public class UploadHandler
{
    private const long MaxLogoSize = 5 * 1024 * 1024; // 5MB
    private const long MaxMaterialSize = 100 * 1024 * 1024; // 100MB

    private static readonly string[] ValidLogoExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private static readonly string[] ValidMaterialExtensions = [".pdf", ".doc", ".docx", ".txt", ".mp4", ".webm", ".mov", ".avi"];

    public async Task<UploadResult> UploadLogoAsync(IFormFile file)
    {
        var result = await ValidateFile(file, ValidLogoExtensions, MaxLogoSize);
        if (!result.Success)
            return result;

        // Additional logo-specific validation
        if (!file.ContentType.StartsWith("image/"))
        {
            return new UploadResult
            {
                Success = false,
                Message = "Invalid file type for logo. Only image files are allowed."
            };
        }

        result.MaterialType = MaterialType.Image;
        return result;
    }

    public async Task<UploadResult> UploadMaterialAsync(IFormFile file)
    {
        var result = await ValidateFile(file, ValidMaterialExtensions, MaxMaterialSize);
        if (!result.Success)
            return result;

        // Set material type based on file extension
        result.MaterialType = Material.GetMaterialTypeFromFileName(file.FileName);
        return result;
    }

    private async Task<UploadResult> ValidateFile(IFormFile file, string[] validExtensions, long maxSize)
    {
        if (file == null || file.Length == 0)
        {
            return new UploadResult { Success = false, Message = "No file uploaded." };
        }

        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !validExtensions.Contains(extension))
        {
            return new UploadResult
            {
                Success = false,
                Message = $"Invalid file extension. Valid extensions are: {string.Join(", ", validExtensions)}"
            };
        }

        if (file.Length > maxSize)
        {
            return new UploadResult
            {
                Success = false,
                Message = $"File size is too large. Maximum allowed size is {maxSize / (1024 * 1024)}MB."
            };
        }

        var fileName = $"{Guid.NewGuid()}{extension}";

        // Read file content
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        return new UploadResult
        {
            Success = true,
            FileName = fileName,
            ContentType = file.ContentType,
            FileSize = file.Length
        };
    }
}