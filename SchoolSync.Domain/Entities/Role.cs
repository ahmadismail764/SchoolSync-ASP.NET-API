namespace SchoolSync.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    // Navigation Property
    public List<User> Users { get; set; } = [];
}
