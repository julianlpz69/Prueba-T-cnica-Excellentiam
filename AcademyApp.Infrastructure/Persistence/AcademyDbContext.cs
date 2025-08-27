using AcademyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace AcademyApp.Infrastructure.Persistence
{
    public class AcademyDbContext(DbContextOptions<AcademyDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AcademyDbContext).Assembly);
        }
    }
}
