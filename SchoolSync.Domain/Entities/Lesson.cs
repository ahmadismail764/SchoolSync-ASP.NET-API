namespace SchoolSync.Domain.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SubjectId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Subject Subject { get; set; } = null!;
    public List<Material> Materials { get; set; } = [];
}
