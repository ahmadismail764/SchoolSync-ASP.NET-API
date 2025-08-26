namespace SchoolSync.Domain.Entities;

public class Material
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Data { get; set; } = null!;
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
}
