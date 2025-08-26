using System.ComponentModel.DataAnnotations;
namespace SchoolSync.App.DTOs.SchoolYear;

public class UpdateSchoolYearDto
{
    [StringLength(50)]
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolId { get; set; }
}
