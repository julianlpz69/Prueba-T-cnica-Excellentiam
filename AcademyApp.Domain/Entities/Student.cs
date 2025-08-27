using AcademyApp.Domain.Enums;


namespace AcademyApp.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;      
        public int Age { get; set; }                      
        public StudentStatus Status { get; set; }             
        public DateTime EnrollmentDate { get; set; }          
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
