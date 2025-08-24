namespace SchoolSync.Domain.Entities;

public class Term
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SchoolYearId { get; set; }

    public bool IsActive { get; set; } = true;
    public SchoolYear SchoolYear { get; set; } = null!;
    public List<Enrollment> Enrollments { get; set; } = new();
}