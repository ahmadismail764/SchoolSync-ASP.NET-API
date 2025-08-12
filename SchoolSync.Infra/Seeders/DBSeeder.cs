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

    internal DbSet<StudentDetails> StudentDetails { get; set; } = context.StudentDetails;
    internal DbSet<Subject> Subjects { get; set; } = context.Subjects;
    internal DbSet<Term> Terms { get; set; } = context.Terms;
    internal DbSet<Enrollment> Enrollments { get; set; } = context.Enrollments;

    internal DbSet<Role> Roles { get; set; } = context.Roles;

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedOrganizationsSchoolsUsersSubjectsAsync();
        await SeedStudentDetailsAsync();
        await SeedEnrollmentsAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (!await Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Student" },
                new Role { Name = "Teacher" },
                new Role { Name = "Admin" }
            };
            await Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedOrganizationsSchoolsUsersSubjectsAsync()
    {
        if (await Organizations.AnyAsync())
            return;

        // Get role IDs
        var studentRole = await Roles.FirstAsync(r => r.Name == "Student");
        var teacherRole = await Roles.FirstAsync(r => r.Name == "Teacher");

        // 2 orgs
        var orgs = new List<Organization>
        {
            new Organization { Name = "Springfield Education District", Description = "A leading educational organization serving the Springfield community", Address = "100 Education Blvd, Springfield, IL 62701", PhoneNumber = "+1-217-555-0100", Email = "info@springfieldedu.org" },
            new Organization { Name = "Metro Learning Network", Description = "Innovative education solutions for metropolitan areas", Address = "200 Metro Plaza, Springfield, IL 62702", PhoneNumber = "+1-217-555-0200", Email = "contact@metrolearning.org" }
        };
        await Organizations.AddRangeAsync(orgs);
        await context.SaveChangesAsync();

        // 1 school per org
        var schools = new List<School>
        {
            new School { Name = "Lincoln High School", Address = "123 Main St, Springfield, IL 62701", PhoneNumber = "+1-217-555-0123", Email = "admin@lincolnhigh.edu", OrganizationId = orgs[0].Id },
            new School { Name = "Washington Elementary", Address = "456 Oak Ave, Springfield, IL 62702", PhoneNumber = "+1-217-555-0456", Email = "office@washington.edu", OrganizationId = orgs[1].Id }
        };
        await Schools.AddRangeAsync(schools);
        await context.SaveChangesAsync();

        // 3 teachers and 3 students per school
        var users = new List<User>();
        int userCounter = 1;
        foreach (var school in schools)
        {
            for (int i = 1; i <= 3; i++)
            {
                users.Add(new User
                {
                    FullName = $"Teacher {i} ({school.Name})",
                    Email = $"teacher{i}.{school.Name.Replace(" ", "").ToLower()}@school.edu",
                    Username = $"teacher{i}_{school.Name.Replace(" ", "").ToLower()}",
                    SchoolId = school.Id,
                    RoleId = teacherRole.Id
                });
            }
            for (int i = 1; i <= 3; i++)
            {
                users.Add(new User
                {
                    FullName = $"Student {i} ({school.Name})",
                    Email = $"student{i}.{school.Name.Replace(" ", "").ToLower()}@school.edu",
                    Username = $"student{i}_{school.Name.Replace(" ", "").ToLower()}",
                    SchoolId = school.Id,
                    RoleId = studentRole.Id
                });
            }
        }
        await Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // 3 subjects per school, each assigned to a teacher from that school
        var allTeachers = await Users.Where(u => u.RoleId == teacherRole.Id).ToListAsync();
        var subjects = new List<Subject>();
        foreach (var school in schools)
        {
            var teachers = allTeachers.Where(t => t.SchoolId == school.Id).ToList();
            for (int i = 1; i <= 3; i++)
            {
                subjects.Add(new Subject
                {
                    Name = $"Subject {i} ({school.Name})",
                    Code = $"SUBJ{i}{school.Id}",
                    Credits = 3,
                    SchoolId = school.Id,
                    TeacherId = teachers[(i-1)%teachers.Count].Id
                });
            }
        }
        await Subjects.AddRangeAsync(subjects);
        await context.SaveChangesAsync();

        // Add a school year and terms for each school
        foreach (var school in schools)
        {
            var schoolYear = new SchoolYear
            {
                Year = 2024,
                StartDate = new DateTime(2024, 8, 15),
                EndDate = new DateTime(2025, 6, 15),
                SchoolId = school.Id
            };
            await SchoolYears.AddAsync(schoolYear);
            await context.SaveChangesAsync();
            var terms = new List<Term>
            {
                new Term { Name = $"Fall 2024 ({school.Name})", StartDate = new DateTime(2024, 8, 15), EndDate = new DateTime(2024, 12, 20), SchoolYearId = schoolYear.Id },
                new Term { Name = $"Spring 2025 ({school.Name})", StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15), SchoolYearId = schoolYear.Id }
            };
            await Terms.AddRangeAsync(terms);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedStudentDetailsAsync()
    {
        if (await StudentDetails.AnyAsync())
            return;
        var studentRole = await Roles.FirstAsync(r => r.Name == "Student");
        var students = await Users.Where(u => u.RoleId == studentRole.Id).ToListAsync();
        var details = new List<StudentDetails>();
        foreach (var student in students)
        {
            details.Add(new StudentDetails
            {
                StudentId = student.Id,
                GPA = 4.0m,
                AttendanceRate = 1.0m,
                ParticipationRating = 1.0m,
                IsActive = true
            });
        }
        if (details.Count > 0)
        {
            await StudentDetails.AddRangeAsync(details);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedEnrollmentsAsync()
    {
        if (await Enrollments.AnyAsync())
            return;
        var studentRole = await Roles.FirstAsync(r => r.Name == "Student");
        var students = await Users.Where(u => u.RoleId == studentRole.Id).ToListAsync();
        var schools = await Schools.ToListAsync();
        foreach (var school in schools)
        {
            var schoolStudents = students.Where(s => s.SchoolId == school.Id).ToList();
            var schoolSubjects = await Subjects.Where(sub => sub.SchoolId == school.Id).ToListAsync();
            var term = await Terms.FirstAsync(t => t.SchoolYear.SchoolId == school.Id);
            var enrollments = new List<Enrollment>();
            foreach (var student in schoolStudents)
            {
                foreach (var subject in schoolSubjects)
                {
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.Id,
                        SubjectId = subject.Id,
                        TermId = term.Id,
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
