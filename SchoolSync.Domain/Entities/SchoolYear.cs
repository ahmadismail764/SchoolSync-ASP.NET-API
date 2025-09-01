namespace SchoolSync.Domain.Entities;

public class SchoolYear
{
    public int Id { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SchoolId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public School School { get; set; } = null!;
    public List<Term> Terms { get; set; } = [];
}