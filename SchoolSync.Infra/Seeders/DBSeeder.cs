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
            var subjects = await Subjects.Where(s => s.Id > 0).ToListAsync();

            if (subjects.Count == 0)
            {
                throw new InvalidOperationException("No subjects found to create lessons.");
            }

            var lessons = new List<Lesson>
            {
                new() {
                    Title = "Lesson 1",
                    Description = "Intro Lesson",
                    SubjectId = subjects[0].Id,
                    IsDeleted = false
                },
                new() {
                    Title = "Lesson 2",
                    Description = "Second Lesson",
                    SubjectId = subjects[0].Id,
                    IsDeleted = false
                }
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
                // Try multiple path resolutions for PDF
                var pdfPaths = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedFiles", "AhmadIsmail-Resume.pdf"),
                    Path.Combine(Directory.GetCurrentDirectory(), "SeedFiles", "AhmadIsmail-Resume.pdf"),
                    Path.Combine(Directory.GetCurrentDirectory(), "SchoolSync.Infra", "SeedFiles", "AhmadIsmail-Resume.pdf"),
                    Path.Combine(Directory.GetCurrentDirectory(), "..", "SchoolSync.Infra", "SeedFiles", "AhmadIsmail-Resume.pdf")
                };

                foreach (var pdfPath in pdfPaths)
                {
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
                            LessonId = lesson.Id,
                            IsDeleted = false
                        };
                        await context.Materials.AddAsync(material);
                        break; // Exit loop after finding and adding first valid file
                    }
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
                new() {
                    Name = "Greenfield Academy Trust",
                    Address = "101 Green St, Cityville",
                    PhoneNumber = "+1-555-1000",
                    Email = "info@greenfield.org",
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Blue River Schools",
                    Address = "202 Blue River Rd, Townsville",
                    PhoneNumber = "+1-555-2000",
                    Email = "contact@blueriver.edu",
                    IsActive = true,
                    IsDeleted = false
                }
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

            // Try to load logo, fallback to empty array if file doesn't exist
            byte[] logoBytes = Array.Empty<byte>();

            // Try multiple path resolutions for logo
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedFiles", "image.png");
            var alternatePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "SeedFiles", "image.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "SchoolSync.Infra", "SeedFiles", "image.png"),
                Path.Combine(Directory.GetCurrentDirectory(), "..", "SchoolSync.Infra", "SeedFiles", "image.png")
            };

            // Try main path first, then alternatives
            if (File.Exists(logoPath))
            {
                logoBytes = await File.ReadAllBytesAsync(logoPath);
            }
            else
            {
                foreach (var altPath in alternatePaths)
                {
                    if (File.Exists(altPath))
                    {
                        logoBytes = await File.ReadAllBytesAsync(altPath);
                        break;
                    }
                }
            }

            var schools = new List<School>
            {
                new() {
                    Name = "Greenfield High",
                    Address = "1 School Lane",
                    PhoneNumber = "+1-555-1100",
                    Email = "admin@greenfieldhigh.edu",
                    OrganizationId = orgs[0].Id,
                    Logo = logoBytes,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Blue River Primary",
                    Address = "2 River Rd",
                    PhoneNumber = "+1-555-2100",
                    Email = "office@blueriverprimary.edu",
                    OrganizationId = orgs[1].Id,
                    Logo = logoBytes,
                    IsActive = true,
                    IsDeleted = false
                }
            };
            await Schools.AddRangeAsync(schools);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedUsersAsync()
    {
        if (!await Users.AnyAsync())
        {
            // Get schools and roles from database to ensure we have proper IDs
            var schools = await Schools.Where(s => !s.IsDeleted && s.Id > 0).OrderBy(s => s.Id).ToListAsync();
            var roles = await Roles.ToListAsync();

            if (schools.Count == 0)
            {
                throw new InvalidOperationException("No schools found in database. Schools must be seeded before users.");
            }

            if (schools.Count < 2)
            {
                throw new InvalidOperationException("At least 2 schools must exist to properly seed users.");
            }

            var teacherRole = roles.FirstOrDefault(r => r.Name == "Teacher");
            var studentRole = roles.FirstOrDefault(r => r.Name == "Student");

            if (teacherRole == null || studentRole == null)
            {
                throw new InvalidOperationException("Teacher and Student roles must exist to seed users.");
            }

            // Microsoft Identity compatible hash for "Password123!" 
            var password = "AQAAAAEAACcQAAAAEKXdFh8HFvTdkHQC3rJz5jXYCgKs2LmOZftLkl2F9qRAFg5VQJ7Z3sF8BaFKL9TqxA==";
            var users = new List<User>
            {
                // Teachers
                new() {
                    FullName = "Alice Smith",
                    Email = "alice.smith@greenfieldhigh.edu",
                    Username = "asmith",
                    SchoolId = schools[0].Id, // This should now be a valid ID
                    RoleId = teacherRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    FullName = "Bob Johnson",
                    Email = "bob.johnson@greenfieldhigh.edu",
                    Username = "bjohnson",
                    SchoolId = schools[0].Id, // This should now be a valid ID
                    RoleId = teacherRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    FullName = "Carol White",
                    Email = "carol.white@blueriverprimary.edu",
                    Username = "cwhite",
                    SchoolId = schools[1].Id,
                    RoleId = teacherRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                // Students
                new() {
                    FullName = "David Lee",
                    Email = "david.lee@greenfieldhigh.edu",
                    Username = "dlee",
                    SchoolId = schools[0].Id,
                    RoleId = studentRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    FullName = "Eva Brown",
                    Email = "eva.brown@greenfieldhigh.edu",
                    Username = "ebrown",
                    SchoolId = schools[0].Id,
                    RoleId = studentRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    FullName = "Frank Green",
                    Email = "frank.green@blueriverprimary.edu",
                    Username = "fgreen",
                    SchoolId = schools[1].Id,
                    RoleId = studentRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    FullName = "Grace Kim",
                    Email = "grace.kim@blueriverprimary.edu",
                    Username = "gkim",
                    SchoolId = schools[1].Id,
                    RoleId = studentRole.Id,
                    PasswordHash = password,
                    IsActive = true,
                    IsDeleted = false
                }
            };
            await Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedStudentDetailsAsync()
    {
        if (!await StudentDetails.AnyAsync())
        {
            var students = await Users.Include(u => u.Role).Where(u => u.Role.Name == "Student").ToListAsync();

            if (students.Count == 0)
            {
                throw new InvalidOperationException("No students found to create student details.");
            }

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
            var schools = await Schools.Where(s => !s.IsDeleted && s.Id > 0).OrderBy(s => s.Id).ToListAsync();
            var teachers = await Users.Include(u => u.Role).Where(u => u.Role.Name == "Teacher" && u.SchoolId > 0).ToListAsync();

            if (schools.Count == 0 || schools.Count < 2)
            {
                throw new InvalidOperationException("At least 2 schools must exist to seed subjects.");
            }

            if (teachers.Count == 0 || teachers.Count < 2)
            {
                throw new InvalidOperationException("At least 2 teachers must exist to seed subjects.");
            }

            var subjects = new List<Subject>
            {
                new() {
                    Name = "Mathematics",
                    Code = "MATH101",
                    Credits = 3,
                    SchoolId = schools[0].Id,
                    TeacherId = teachers[0].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "English",
                    Code = "ENG101",
                    Credits = 2,
                    SchoolId = schools[0].Id,
                    TeacherId = teachers[1].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Science",
                    Code = "SCI101",
                    Credits = 3,
                    SchoolId = schools[1].Id,
                    TeacherId = teachers.Count > 2 ? teachers[2].Id : teachers[0].Id,
                    IsActive = true,
                    IsDeleted = false
                }
            };
            await Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedSchoolYearsAsync()
    {
        if (!await SchoolYears.AnyAsync())
        {
            var schools = await Schools.Where(s => !s.IsDeleted && s.Id > 0).OrderBy(s => s.Id).ToListAsync();

            if (schools.Count == 0 || schools.Count < 2)
            {
                throw new InvalidOperationException("At least 2 schools must exist to seed school years.");
            }

            var years = new List<SchoolYear>
            {
                new() {
                    Year = 2024,
                    StartDate = new DateTime(2024, 8, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    SchoolId = schools[0].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Year = 2024,
                    StartDate = new DateTime(2024, 8, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    SchoolId = schools[1].Id,
                    IsActive = true,
                    IsDeleted = false
                }
            };
            await SchoolYears.AddRangeAsync(years);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedTermsAsync()
    {
        if (!await Terms.AnyAsync())
        {
            var years = await SchoolYears.Where(y => !y.IsDeleted && y.Id > 0).OrderBy(y => y.Id).ToListAsync();

            if (years.Count == 0 || years.Count < 2)
            {
                throw new InvalidOperationException("At least 2 school years must exist to seed terms.");
            }

            var terms = new List<Term>
            {
                new() {
                    Name = "Fall 2024",
                    StartDate = new DateTime(2024, 8, 15),
                    EndDate = new DateTime(2024, 12, 20),
                    SchoolYearId = years[0].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Spring 2025",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    SchoolYearId = years[0].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Fall 2024",
                    StartDate = new DateTime(2024, 8, 15),
                    EndDate = new DateTime(2024, 12, 20),
                    SchoolYearId = years[1].Id,
                    IsActive = true,
                    IsDeleted = false
                },
                new() {
                    Name = "Spring 2025",
                    StartDate = new DateTime(2025, 1, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    SchoolYearId = years[1].Id,
                    IsActive = true,
                    IsDeleted = false
                }
            };
            await Terms.AddRangeAsync(terms);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedEnrollmentsAsync()
    {
        if (!await Enrollments.AnyAsync())
        {
            var students = await Users.Include(u => u.Role).Where(u => u.Role.Name == "Student" && u.SchoolId > 0).ToListAsync();
            var subjects = await Subjects.Where(s => s.SchoolId > 0).ToListAsync();
            var terms = await Terms.Include(t => t.SchoolYear).Where(t => t.SchoolYear.SchoolId > 0).ToListAsync();
            var enrollments = new List<Enrollment>();

            if (students.Count == 0)
            {
                throw new InvalidOperationException("No students found to enroll.");
            }

            if (!subjects.Any())
            {
                throw new InvalidOperationException("No subjects found for enrollment.");
            }

            if (!terms.Any())
            {
                throw new InvalidOperationException("No terms found for enrollment.");
            }

            foreach (var student in students)
            {
                // Enroll each student in subjects of their school for the current term
                var studentSubjects = subjects.Where(s => s.SchoolId == student.SchoolId).ToList();
                var studentTerms = terms.Where(t => t.SchoolYear.SchoolId == student.SchoolId).ToList();

                if (studentTerms.Count != 0 && studentSubjects.Count != 0)
                {
                    // Use the first (current) term for the student's school
                    var currentTerm = studentTerms.OrderBy(t => t.StartDate).First();

                    foreach (var subject in studentSubjects)
                    {
                        // Set enrollment date to be within the term period
                        var enrollmentDate = currentTerm.StartDate.AddDays(7); // Enroll 1 week after term starts

                        enrollments.Add(new Enrollment
                        {
                            StudentId = student.Id,
                            SubjectId = subject.Id,
                            TermId = currentTerm.Id,
                            EnrollmentDate = enrollmentDate,
                            IsActive = true,
                            IsDeleted = false
                        });
                    }
                }
            }
            await Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();
        }
    }
}