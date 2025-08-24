namespace SchoolSync.Domain.Entities;

public class School
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int OrganizationId { get; set; }

    // Delete magic happens with the following property
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Organization Organization { get; set; } = null!;
    public List<User> PeopleHere { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
    public List<SchoolYear> SchoolYears { get; set; } = new();
}
