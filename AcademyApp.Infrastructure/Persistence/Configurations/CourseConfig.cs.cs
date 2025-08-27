using AcademyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AcademyApp.Infrastructure.Persistence.Configurations
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> b)
        {
            b.ToTable("Courses", t => t.HasTrigger("trg_Courses_Update"));
            b.HasKey(x => x.Id);

            b.Property(x => x.Name).IsRequired().HasMaxLength(150);
            b.Property(x => x.Credits).IsRequired();
            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.StartDate).IsRequired();

            b.HasMany(x => x.Students)
             .WithOne(s => s.Course!)
             .HasForeignKey(s => s.CourseId)
             .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
