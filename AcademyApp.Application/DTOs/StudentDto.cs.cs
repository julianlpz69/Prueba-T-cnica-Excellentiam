

namespace AcademyApp.Application.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public int Status { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int CourseId { get; set; }

        public StudentDto() { }

        public StudentDto(int id, string fullName, int age, int status, DateTime enrollmentDate, int courseId)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            Status = status;
            EnrollmentDate = enrollmentDate;
            CourseId = courseId;
        }
    };

}
