using Microsoft.EntityFrameworkCore;

using SchoolSync.Domain.Entities;
namespace SchoolSync.Infra.Persistence;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SchoolYear> SchoolYears { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enrollment relations start here /////////////////////////////
        // 1. Enrollment has foreign key to student
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Enrollment has foreign key to subject
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Subject)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        // 3. Enrollment has foreign key to term
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Term)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.TermId)
            .OnDelete(DeleteBehavior.Restrict);

        // 4. Enrollment has composite pkey
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.SubjectId, e.TermId })
            .IsUnique();
        // Enrollment relations end here /////////////////////////////
        
        // Subject relations start here /////////////////////////////
        // 1. Subject has foreign key to school
        modelBuilder.Entity<Subject>()
            .HasOne(sub => sub.School)
            .WithMany(sc => sc.Subjects)
            .HasForeignKey(sub => sub.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
        // Subject relations end here /////////////////////////////

        // School relations start here /////////////////////////////
        // 1. School has foreign key to Organizatoin
        modelBuilder.Entity<School>()
                    .HasOne(s => s.Organization)
                    .WithMany(o => o.Schools)
                    .HasForeignKey(s => s.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
        // School relations end here ///////////////////////////////
        
        // User relations start here /////////////////////////////
        // 1. User has foreign key to School
        modelBuilder.Entity<User>()
                    .HasOne(s => s.School)
                    .WithMany(o => o.PeopleHere)
                    .HasForeignKey(s => s.SchoolId)
                    .OnDelete(DeleteBehavior.Restrict);
        // School relations end here ///////////////////////////////

        // StudentDetails relations start here /////////////////////////////
        // 1. StudentDetails has foreign key to User
        modelBuilder.Entity<StudentDetails>()
            .HasOne(s => s.Student)
            .WithOne(o => o.Details)
            .HasForeignKey<StudentDetails>(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        // StudentDetails relations end here ///////////////////////////////

        // SchoolYear relations start here /////////////////////////////
        // 1. SchoolYear has foreign key to School
        modelBuilder.Entity<SchoolYear>()
                    .HasOne(s => s.School)
                    .WithMany(o => o.SchoolYears)
                    .HasForeignKey(s => s.SchoolId)
                    .OnDelete(DeleteBehavior.Restrict);
        // SchoolYear relations end here /////////////////////////////
        // Term relations start here /////////////////////////////
        // 1. Term has foreign key to SchoolYear
        modelBuilder.Entity<Term>()
                    .HasOne(s => s.SchoolYear)
                    .WithMany(o => o.Terms)
                    .HasForeignKey(s => s.SchoolYearId)
                    .OnDelete(DeleteBehavior.Restrict);
        // SchoolYear relations end here /////////////////////////////
        base.OnModelCreating(modelBuilder);
    }
}
