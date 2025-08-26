namespace SchoolSync.Domain.Entities;

public enum MaterialType
{
    PDF,
    Video,
    Image,
    Document,
    Other
}

public class Material
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public MaterialType Type { get; set; }
    public byte[] Data { get; set; } = null!;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }

    // Relationships
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;

    // Helper method to determine material type from file extension
    public static MaterialType GetMaterialTypeFromFileName(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => MaterialType.PDF,
            ".mp4" or ".webm" or ".mov" or ".avi" => MaterialType.Video,
            ".jpg" or ".jpeg" or ".png" or ".gif" => MaterialType.Image,
            ".doc" or ".docx" or ".txt" or ".rtf" => MaterialType.Document,
            _ => MaterialType.Other
        };
    }
}
