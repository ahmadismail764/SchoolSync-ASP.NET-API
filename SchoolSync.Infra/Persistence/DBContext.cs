using Microsoft.EntityFrameworkCore;
using SchoolSync.Domain.Entities;
namespace SchoolSync.Infra.Persistence;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SchoolYear> SchoolYears { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }

    public DbSet<Role> Roles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Organization - School (1:M)
        modelBuilder.Entity<School>()
            .HasOne(s => s.Organization)
            .WithMany(o => o.Schools)
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        // School - User (1:M)
        modelBuilder.Entity<User>()
            .HasOne(u => u.School)
            .WithMany(s => s.PeopleHere)
            .HasForeignKey(u => u.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        // School - Subject (1:M)
        modelBuilder.Entity<Subject>()
            .HasOne(sub => sub.School)
            .WithMany(sch => sch.Subjects)
            .HasForeignKey(sub => sub.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        // School - SchoolYear (1:M)
        modelBuilder.Entity<SchoolYear>()
            .HasOne(sy => sy.School)
            .WithMany(s => s.SchoolYears)
            .HasForeignKey(sy => sy.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        // SchoolYear - Term (1:M)
        modelBuilder.Entity<Term>()
            .HasOne(t => t.SchoolYear)
            .WithMany(sy => sy.Terms)
            .HasForeignKey(t => t.SchoolYearId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - Role (M:1)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - Subject (M:M, as Teacher)
        modelBuilder.Entity<Subject>()
            .HasOne(sub => sub.Teacher)
            .WithMany(u => u.Subjects)
            .HasForeignKey(sub => sub.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - Enrollment (1:M, as Student)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enrollment - Subject (M:1)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Subject)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enrollment - Term (M:1)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Term)
            .WithMany(t => t.Enrollments)
            .HasForeignKey(e => e.TermId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enrollment composite index (StudentId, SubjectId, TermId)
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.SubjectId, e.TermId })
            .IsUnique();

        // User - StudentDetails (1:1)
        modelBuilder.Entity<StudentDetails>()
            .HasOne(sd => sd.Student)
            .WithOne(u => u.Details)
            .HasForeignKey<StudentDetails>(sd => sd.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
