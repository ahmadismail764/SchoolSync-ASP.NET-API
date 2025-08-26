using Microsoft.EntityFrameworkCore;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Persistence;
using BCrypt.Net;
namespace SchoolSync.Infra.Seeders;

internal class DBSeeder(DBContext context) : IDBSeeder
{

    internal DbSet<Organization> Organizations { get; set; } = context.Organizations;
    internal DbSet<School> Schools { get; set; } = context.Schools;
    internal DbSet<SchoolYear> SchoolYears { get; set; } = context.SchoolYears;
    internal DbSet<User> Users { get; set; } = context.Users;

    internal DbSet<StudentDetails> StudentDetails { get; set; } = context.StudentDetails;
    internal DbSet<Subject> Subjects { get; set; } = context.Subjects;
    internal DbSet<Term> Terms { get; set; } = context.Terms;
    internal DbSet<Enrollment> Enrollments { get; set; } = context.Enrollments;

    internal DbSet<Role> Roles { get; set; } = context.Roles;

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedOrganizationsAsync();
        await SeedSchoolsAsync();
        await SeedUsersAsync();
        await SeedStudentDetailsAsync();
        await SeedSubjectsAsync();
        await SeedSchoolYearsAsync();
        await SeedTermsAsync();
        await SeedEnrollmentsAsync();
        await SeedLessonsAsync();
        await SeedMaterialsAsync();
    }
    private async Task SeedLessonsAsync()
    {
        if (!context.Lessons.Any())
        {
            var subjects = await Subjects.ToListAsync();
            var lessons = new List<Lesson>
            {
                new() { Title = "Lesson 1", Description = "Intro Lesson", SubjectId = subjects[0].Id },
                new() { Title = "Lesson 2", Description = "Second Lesson", SubjectId = subjects[0].Id }
            };
            await context.Lessons.AddRangeAsync(lessons);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedMaterialsAsync()
    {
        if (!context.Materials.Any())
        {
            // Dummy PDF
            var lesson = await context.Lessons.FirstOrDefaultAsync();
            if (lesson != null)
            {
                var pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "SchoolSync.Infra", "SeedFiles", "AhmadIsmail-Resume.pdf");
                if (File.Exists(pdfPath))
                {
                    var pdfBytes = await File.ReadAllBytesAsync(pdfPath);
                    var material = new Material
                    {
                        FileName = "AhmadIsmail-Resume.pdf",
                        ContentType = "application/pdf",
                        FileType = "pdf",
                        FileSize = pdfBytes.Length,
                        FileData = pdfBytes,
                        UploadDate = DateTime.UtcNow,
                        Description = "Dummy PDF for seeding",
                        LessonId = lesson.Id
                    };
                    await context.Materials.AddAsync(material);
                }
            }
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        if (!await Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "Student" },
                new() { Name = "Teacher" }
            };
            await Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedOrganizationsAsync()
    {
        if (!await Organizations.AnyAsync())
        {
            var orgs = new List<Organization>
            {
                new() { Name = "Greenfield Academy Trust", Address = "101 Green St, Cityville", PhoneNumber = "+1-555-1000", Email = "info@greenfield.org" },
                new() { Name = "Blue River Schools", Address = "202 Blue River Rd, Townsville", PhoneNumber = "+1-555-2000", Email = "contact@blueriver.edu" }
            };
            await Organizations.AddRangeAsync(orgs);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSchoolsAsync()
    {
        if (!await Schools.AnyAsync())
        {
            var orgs = await Organizations.ToListAsync();
            byte[]? logoBytes = null;
            string? logoContentType = null;
            long? logoSize = null;
            DateTime? logoUploadDate = null;
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "SchoolSync.Infra", "SeedFiles", "image.png");
            if (File.Exists(logoPath))
            {
                logoBytes = await File.ReadAllBytesAsync(logoPath);
                logoContentType = "image/png";
                logoSize = logoBytes.Length;
                logoUploadDate = DateTime.UtcNow;
            }
            var schools = new List<School>
            {
                new() { Name = "Greenfield High", Address = "1 School Lane", PhoneNumber = "+1-555-1100", Email = "admin@greenfieldhigh.edu", OrganizationId = orgs[0].Id, Logo = logoBytes, LogoContentType = logoContentType, LogoSize = logoSize, LogoUploadDate = logoUploadDate },
                new() { Name = "Blue River Primary", Address = "2 River Rd", PhoneNumber = "+1-555-2100", Email = "office@blueriverprimary.edu", OrganizationId = orgs[1].Id, Logo = logoBytes, LogoContentType = logoContentType, LogoSize = logoSize, LogoUploadDate = logoUploadDate }
            };
            await Schools.AddRangeAsync(schools);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedUsersAsync()
    {
        if (!await Users.AnyAsync())
        {
            var schools = await Schools.ToListAsync();
            var roles = await Roles.ToListAsync();
            var teacherRole = roles.First(r => r.Name == "Teacher");
            var studentRole = roles.First(r => r.Name == "Student");
            var password = BCrypt.Net.BCrypt.HashPassword("Password123!");
            var users = new List<User>
            {
                // Teachers
                new() { FullName = "Alice Smith", Email = "alice.smith@greenfieldhigh.edu", Username = "asmith", SchoolId = schools[0].Id, RoleId = teacherRole.Id, PasswordHash = password },
                new() { FullName = "Bob Johnson", Email = "bob.johnson@greenfieldhigh.edu", Username = "bjohnson", SchoolId = schools[0].Id, RoleId = teacherRole.Id, PasswordHash = password },
                new() { FullName = "Carol White", Email = "carol.white@blueriverprimary.edu", Username = "cwhite", SchoolId = schools[1].Id, RoleId = teacherRole.Id, PasswordHash = password },
                // Students
                new() { FullName = "David Lee", Email = "david.lee@greenfieldhigh.edu", Username = "dlee", SchoolId = schools[0].Id, RoleId = studentRole.Id, PasswordHash = password },
                new() { FullName = "Eva Brown", Email = "eva.brown@greenfieldhigh.edu", Username = "ebrown", SchoolId = schools[0].Id, RoleId = studentRole.Id, PasswordHash = password },
                new() { FullName = "Frank Green", Email = "frank.green@blueriverprimary.edu", Username = "fgreen", SchoolId = schools[1].Id, RoleId = studentRole.Id, PasswordHash = password },
                new() { FullName = "Grace Kim", Email = "grace.kim@blueriverprimary.edu", Username = "gkim", SchoolId = schools[1].Id, RoleId = studentRole.Id, PasswordHash = password }
            };
            await Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedStudentDetailsAsync()
    {
        if (!await StudentDetails.AnyAsync())
        {
            var students = await Users.Where(u => u.Role.Name == "Student").ToListAsync();
            var details = students.Select(s => new StudentDetails
            {
                StudentId = s.Id,
                GPA = 3.5m,
                AttendanceRate = 0.95m,
                ParticipationRating = 0.9m,
                IsActive = true
            }).ToList();
            await StudentDetails.AddRangeAsync(details);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSubjectsAsync()
    {
        if (!await Subjects.AnyAsync())
        {
            var schools = await Schools.ToListAsync();
            var teachers = await Users.Where(u => u.Role.Name == "Teacher").ToListAsync();
            var subjects = new List<Subject>
            {
                new() { Name = "Mathematics", Code = "MATH101", Credits = 3, SchoolId = schools[0].Id, TeacherId = teachers[0].Id },
                new() { Name = "English", Code = "ENG101", Credits = 2, SchoolId = schools[0].Id, TeacherId = teachers[1].Id },
                new() { Name = "Science", Code = "SCI101", Credits = 3, SchoolId = schools[1].Id, TeacherId = teachers[2].Id }
            };
            await Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSchoolYearsAsync()
    {
        if (!await SchoolYears.AnyAsync())
        {
            var schools = await Schools.ToListAsync();
            var years = new List<SchoolYear>
            {
                new() { Year = 2024, StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2025, 6, 15), SchoolId = schools[0].Id },
                new() { Year = 2024, StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2025, 6, 15), SchoolId = schools[1].Id }
            };
            await SchoolYears.AddRangeAsync(years);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedTermsAsync()
    {
        if (!await Terms.AnyAsync())
        {
            var years = await SchoolYears.ToListAsync();
            var terms = new List<Term>
            {
                new() { Name = "Fall 2024", StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2024, 12, 20), SchoolYearId = years[0].Id },
                new() { Name = "Spring 2025", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15), SchoolYearId = years[0].Id },
                new() { Name = "Fall 2024", StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2024, 12, 20), SchoolYearId = years[1].Id },
                new() { Name = "Spring 2025", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15), SchoolYearId = years[1].Id }
            };
            await Terms.AddRangeAsync(terms);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedEnrollmentsAsync()
    {
        if (!await Enrollments.AnyAsync())
        {
            var students = await Users.Where(u => u.Role.Name == "Student").ToListAsync();
            var subjects = await Subjects.ToListAsync();
            var terms = await Terms.ToListAsync();
            var enrollments = new List<Enrollment>();
            foreach (var student in students)
            {
                // Enroll each student in all subjects of their school for the first term
                var studentSubjects = subjects.Where(s => s.SchoolId == student.SchoolId).ToList();
                var firstTerm = terms.First(t => t.SchoolYear.SchoolId == student.SchoolId);
                foreach (var subject in studentSubjects)
                {
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.Id,
                        SubjectId = subject.Id,
                        TermId = firstTerm.Id,
                        EnrollmentDate = DateTime.UtcNow,
                        IsActive = true
                    });
                }
            }
            await Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();
        }
    }
}