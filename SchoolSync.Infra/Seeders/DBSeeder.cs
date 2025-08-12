using Microsoft.EntityFrameworkCore;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
namespace SchoolSync.Infra.Seeders;

internal class DBSeeder(DBContext context) : IDBSeeder
{

    internal DbSet<Organization> Organizations { get; set; } = context.Organizations;
    internal DbSet<School> Schools { get; set; } = context.Schools;
    internal DbSet<SchoolYear> SchoolYears { get; set; } = context.SchoolYears;
    internal DbSet<User> Users { get; set; } = context.Users;

    internal DbSet<StudentDetails> StudentDetails { get; set; }
    internal DbSet<Subject> Subjects { get; set; } = context.Subjects;
    internal DbSet<Term> Terms { get; set; } = context.Terms;
    internal DbSet<Enrollment> Enrollments { get; set; } = context.Enrollments;

    public async Task SeedAsync()
    {
        await SeedOrganizationsAsync();
        await SeedSchoolsAsync();
        await SeedSchoolYearsAsync();
        await SeedTermsAsync();
        await SeedUsersAsync();        
        await SeedSubjectsAsync();
        await SeedStudentDetailsAsync();
        await SeedEnrollmentsAsync();
    }

    private async Task SeedOrganizationsAsync() {
        if (!await Organizations.AnyAsync())
        {
            var organizations = new List<Organization>
            {
                new Organization
                {
                    Name = "Springfield Education District",
                    Description = "A leading educational organization serving the Springfield community",
                    Address = "100 Education Blvd, Springfield, IL 62701",
                    PhoneNumber = "+1-217-555-0100",
                    Email = "info@springfieldedu.org"
                },
                new Organization
                {
                    Name = "Metro Learning Network",
                    Description = "Innovative education solutions for metropolitan areas",
                    Address = "200 Metro Plaza, Springfield, IL 62702",
                    PhoneNumber = "+1-217-555-0200",
                    Email = "contact@metrolearning.org"
                }
            };

            await Organizations.AddRangeAsync(organizations);
            await context.SaveChangesAsync(); 
        }
    }
    private async Task SeedSchoolsAsync()
    {
        if (!await Schools.AnyAsync())
        {
            var org1 = await Organizations.FirstAsync(o => o.Name == "Springfield Education District");
            var org2 = await Organizations.FirstAsync(o => o.Name == "Metro Learning Network");

            var schools = new List<School>
            {
                new School
                {
                    Name = "Lincoln High School",
                    Address = "123 Main St, Springfield, IL 62701",
                    PhoneNumber = "+1-217-555-0123",
                    Email = "admin@lincolnhigh.edu",
                    OrganizationId = org1.Id
                },
                new School
                {
                    Name = "Washington Elementary",
                    Address = "456 Oak Ave, Springfield, IL 62702",
                    PhoneNumber = "+1-217-555-0456",
                    Email = "office@washington.edu",
                    OrganizationId = org2.Id
                }
            };

            await Schools.AddRangeAsync(schools);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSchoolYearsAsync()
    {
        if (!await SchoolYears.AnyAsync())
        {
            var school1 = await Schools.FirstAsync(s => s.Name == "Lincoln High School");

            var schoolYears = new List<SchoolYear>
            {
                new SchoolYear
                {
                    Year = 2024,
                    StartDate = new DateTime(2024, 8, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    SchoolId = school1.Id
                },
                new SchoolYear
                {
                    Year = 2025,
                    StartDate = new DateTime(2025, 8, 15),
                    EndDate = new DateTime(2026, 6, 15),
                    SchoolId = school1.Id
                }
            };

            await SchoolYears.AddRangeAsync(schoolYears);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedTermsAsync()
    {
        if (!await Terms.AnyAsync())
        {
            var schoolYear2024 = await SchoolYears.FirstAsync(sy => sy.Year == 2024);
            var schoolYear2025 = await SchoolYears.FirstAsync(sy => sy.Year == 2025);

            var terms = new List<Term>
            {
                new Term { Name = "Fall 2024", StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2024, 12, 20), SchoolYearId = schoolYear2024.Id },
                new Term { Name = "Spring 2025", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15), SchoolYearId = schoolYear2024.Id },
                new Term { Name = "Fall 2025", StartDate = new DateTime(2025, 8, 15), EndDate = new DateTime(2025, 12, 20), SchoolYearId = schoolYear2025.Id },
                new Term { Name = "Spring 2026", StartDate = new DateTime(2026, 1, 15), EndDate = new DateTime(2026, 6, 15), SchoolYearId = schoolYear2025.Id }
            };

            await Terms.AddRangeAsync(terms);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedUsersAsync()
    {
        if (!await Users.AnyAsync())
        {
            var school1 = await Schools.FirstAsync(s => s.Name == "Lincoln High School");
            var school2 = await Schools.FirstAsync(s => s.Name == "Washington Elementary");

            var users = new List<User>
            {
                new User
                {
                    FullName = "Michael Wilson", 
                    Email = "michael.wilson@student.lincoln.edu", 
                    Username = "michael.wilson", 
                    SchoolId = school1.Id 
                },
                new User
                {
                    FullName = "Sarah Davis", 
                    Email = "sarah.davis@student.lincoln.edu", 
                    Username = "sarah.davis", 
                    SchoolId = school1.Id 
                },
                new User
                {
                    FullName = "David Miller", 
                    Email = "david.miller@student.lincoln.edu", 
                    Username = "david.miller", 
                    SchoolId = school1.Id 
                },
                new User
                {
                    FullName = "Emma Garcia", 
                    Email = "emma.garcia@student.washington.edu", 
                    Username = "emma.garcia", 
                    SchoolId = school2.Id 
                },
                new User 
                {
                    FullName = "James Martinez", 
                    Email = "james.martinez@student.washington.edu", 
                    Username = "james.martinez", 
                    SchoolId = school2.Id 
                },
                new User
                {
                    FullName = "Oliva Johnson",
                    Email = "olivia.johnson@teacer.lincoln.edu",
                    Username = "olivia.johnson",
                    SchoolId = school1.Id,
                    Role = "Teacher"
                },
                new User
                {
                    FullName = "Liam Brown",
                    Email = "liam.brown@teacer.lincoln.edu",
                    Username = "liam.brown",
                    SchoolId = school2.Id,
                    Role = "Teacher"
                }
            };
            await Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedStudentDetailsAsync()
    {
        if (await StudentDetails.AnyAsync())
        {
            return; // Prevent duplicate inserts
        }

        // Get all users that are students (no Role or Role is null/empty)
        var students = await Users
            .Where(u => u.Role == "Student")
            .ToListAsync();

        var details = new List<StudentDetails>();

        foreach (var student in students)
        {
            details.Add(new StudentDetails
            {
                StudentId = student.Id,
                GPA = 4.0m, // Default GPA
                AttendanceRate = 1.0m, // 100% attendance
                ParticipationRating = 1.0m, // Full participation
                IsActive = true
            });
        }

        if (details.Count > 0)
        {
            await StudentDetails.AddRangeAsync(details);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSubjectsAsync()
    {
        if (!await Subjects.AnyAsync())
        {
            // Seed term-agnostic subjects with SchoolId
            var school1 = await Schools.FirstAsync(s => s.Name == "Lincoln High School");
            var school2 = await Schools.FirstAsync(s => s.Name == "Washington Elementary");

            var subjects = new List<Subject>
            {
                new Subject {
                    Name = "Algebra I",
                    Code = "MATH101",
                    Credits = 3,
                    SchoolId = school1.Id
                },
                new Subject {
                    Name = "English Literature",
                    Code = "ENG201",
                    Credits = 3,
                    SchoolId = school1.Id
                },
                new Subject {
                    Name = "Chemistry",
                    Code = "SCI301",
                    Credits = 4,
                    SchoolId = school1.Id
                },
                new Subject { 
                    Name = "Elementary Math",
                    Code = "MATH001",
                    Credits = 2,
                    SchoolId = school2.Id
                },
                new Subject {
                    Name = "Reading Fundamentals",
                    Code = "ENG001",
                    Credits = 2,
                    SchoolId = school2.Id
                }
            };

            await Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync(); // Save to get auto-generated IDs
        }
    }

    private async Task SeedEnrollmentsAsync()
    {
        // Prevent duplicate inserts when seeding multiple times
        if (await Enrollments.AnyAsync())
        {
            return;
        }
        // Get all the saved entities with their IDs
        var students = await Users.Where(s => s.Role == "Student").ToListAsync();
        var subjects = await Subjects.ToListAsync();
        var fallTerm = await Terms.FirstAsync(t => t.Name == "Fall 2024");
        var springTerm = await Terms.FirstAsync(t => t.Name == "Spring 2025");

        var enrollments = new List<Enrollment>
        {
            // High school students (Michael, Sarah, David)
            new Enrollment {
                StudentId = students.First(s => s.FullName == "Michael Wilson").Id,
                SubjectId = subjects.First(s => s.Code == "MATH101").Id,
                TermId = fallTerm.Id,
                EnrollmentDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true
            },
            new Enrollment {
                StudentId = students.First(s => s.FullName == "Sarah Davis").Id,
                SubjectId = subjects.First(s => s.Code == "ENG201").Id,
                TermId = fallTerm.Id,
                EnrollmentDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true
            },
            
            // Elementary students (Emma, James)
            new Enrollment {
                StudentId = students.First(s => s.FullName == "Emma Garcia").Id,
                SubjectId = subjects.First(s => s.Code == "MATH001").Id,
                TermId = fallTerm.Id,
                EnrollmentDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true
            },
            new Enrollment {
                StudentId = students.First(s => s.FullName == "James Martinez").Id,
                SubjectId = subjects.First(s => s.Code == "ENG001").Id,
                TermId = fallTerm.Id,
                EnrollmentDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true
            }
        };
        await Enrollments.AddRangeAsync(enrollments);
        await context.SaveChangesAsync();
    }
}
