using System.ComponentModel.DataAnnotations;
namespace SchoolSync.App.DTOs.SchoolYear;

public class UpdateSchoolYearDto
{
    public int? Year { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SchoolId { get; set; }
}
