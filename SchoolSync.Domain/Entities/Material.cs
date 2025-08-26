namespace SchoolSync.Domain.Entities;

public class Material
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public byte[] FileData { get; set; } = null!;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }

    // Relationships
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;

}
