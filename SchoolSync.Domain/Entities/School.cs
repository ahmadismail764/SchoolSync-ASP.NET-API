namespace SchoolSync.Domain.Entities;

public class School
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
    public byte[]? Logo { get; set; }
    public bool IsActive { get; set; } = true;
    public Organization Organization { get; set; } = null!;
    public List<User> PeopleHere { get; set; } = [];
    public List<Subject> Subjects { get; set; } = [];
    public List<SchoolYear> SchoolYears { get; set; } = [];
}
