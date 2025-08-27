using AcademyApp.Domain.Enums;


namespace AcademyApp.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;          
        public int Credits { get; set; }                      
        public CourseCategory Category { get; set; }          
        public DateTime StartDate { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
