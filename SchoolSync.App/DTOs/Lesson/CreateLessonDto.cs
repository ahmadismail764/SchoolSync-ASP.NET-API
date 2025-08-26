namespace SchoolSync.App.DTOs.Lesson;

public class CreateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SubjectId { get; set; }
}
