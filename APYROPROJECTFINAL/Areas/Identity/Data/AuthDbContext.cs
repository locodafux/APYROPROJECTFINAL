using APYROPROJECTFINAL.Areas.Identity.Data;
using APYROPROJECTFINAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APYROPROJECTFINAL.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentClassroomDB> Student_Clasrooms { get; set; }
    public DbSet<ClassroomDB> ClassroomDBS { get; set; }
    public DbSet<Educator> Educators { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ApplicationUser> Accounts { get; set; }

    public DbSet<AttendanceReportDatanew> AttendanceReportDatanew { get; set; }


    public DbSet<AttendanceReportData> AttendanceReportData { get; set; }

    public DbSet<Userlogs>Userlogs { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);



        builder.Entity<Student>(entity => { entity.ToTable("Students"); });
        builder.Entity<Educator>(entity => { entity.ToTable("Educators"); });
        builder.Entity<ApplicationUser>(entity => { entity.ToTable("Accounts"); });





    }
}
