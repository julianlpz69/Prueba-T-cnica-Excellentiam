using AcademyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AcademyApp.Infrastructure.Persistence.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> b)
        {
            b.ToTable("Students", t => t.HasTrigger("trg_Students_Update"));

            b.HasKey(x => x.Id);

            b.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            b.Property(x => x.Age).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.EnrollmentDate).IsRequired();

            b.HasIndex(x => new { x.CourseId, x.FullName }); 
        }
    }
}
