namespace AcademyApp.Application.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int Category { get; set; }
        public DateTime StartDate { get; set; }

        public CourseDto() { }

        public CourseDto(int id, string name, int credits, int category, DateTime startDate)
        {
            Id = id;
            Name = name;
            Credits = credits;
            Category = category;
            StartDate = startDate;
        }


    }
}
