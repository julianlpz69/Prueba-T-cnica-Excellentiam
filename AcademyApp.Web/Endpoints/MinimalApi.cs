using AcademyApp.Application.Interfaces;
using AcademyApp.Application.Pagination;
using AcademyApp.Domain.Interfaces;
using ClosedXML.Excel;

namespace AcademyApp.Web.Endpoints
{
    public static class MinimalApi
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            // -------- API COURSES
            app.MapGet("/api/courses", async (ICourseService svc) =>
            {
                var result = await svc.GetAsync(new QueryParams(1, 100));
                return Results.Ok(result.Items);
            });

            // -------- API STUDENTS (by courseId)
            app.MapGet("/api/students", async (int courseId, IStudentService svc) =>
            {
                if (courseId <= 0) return Results.BadRequest("courseId is required and must be > 0.");
                var result = await svc.GetByCourseAsync(courseId, new QueryParams(1, 100));
                return Results.Ok(result.Items);
            });

            // -------- EXPORT COURSES
            app.MapGet("/export/courses.xlsx", async (
                string? search, string? sortBy, bool? desc, int? pageSize,
                ICourseService courseSvc, IExcelExportService excelSvc) =>
            {
                var qp = new QueryParams(1, pageSize ?? 1000, sortBy, desc ?? false, search);
                var result = await courseSvc.GetAsync(qp);

                var file = excelSvc.ExportCourses(result.Items.Select(c => new Domain.Entities.Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    Credits = c.Credits,
                    Category = (Domain.Enums.CourseCategory)c.Category,
                    StartDate = c.StartDate
                }));

                return Results.File(file,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "courses.xlsx");
            });

            // -------- EXPORT STUDENTS
            app.MapGet("/export/students.xlsx", async (
                int? courseId, string? search, string? sortBy, bool? desc, int? pageSize,
                IStudentService studentSvc, ICourseService courseSvc, IExcelExportService excelSvc) =>
            {
                var qp = new QueryParams(1, pageSize ?? 5000, sortBy, desc ?? false, search);

                if (courseId.HasValue && courseId > 0)
                {
                    var course = await courseSvc.GetByIdAsync(courseId.Value);
                    if (course is null) return Results.NotFound($"Course {courseId} not found.");

                    var result = await studentSvc.GetByCourseAsync(courseId.Value, qp);

                    var file = excelSvc.ExportStudents(
                        result.Items.Select(s => new Domain.Entities.Student
                        {
                            Id = s.Id,
                            FullName = s.FullName,
                            Age = s.Age,
                            Status = (Domain.Enums.StudentStatus)s.Status,
                            EnrollmentDate = s.EnrollmentDate,
                            CourseId = s.CourseId
                        }),
                        new Domain.Entities.Course
                        {
                            Id = course.Id,
                            Name = course.Name,
                            Credits = course.Credits,
                            Category = (Domain.Enums.CourseCategory)course.Category,
                            StartDate = course.StartDate
                        });

                    return Results.File(file,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"students_course_{courseId}.xlsx");
                }
                else
                {
                    var result = await studentSvc.GetAsync(qp);

                    var file = excelSvc.ExportStudents(
                        result.Items.Select(s => new Domain.Entities.Student
                        {
                            Id = s.Id,
                            FullName = s.FullName,
                            Age = s.Age,
                            Status = (Domain.Enums.StudentStatus)s.Status,
                            EnrollmentDate = s.EnrollmentDate,
                            CourseId = s.CourseId
                        }),
                        new Domain.Entities.Course { Id = 0, Name = "All Courses" }
                    );

                    return Results.File(file,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "students_all.xlsx");
                }
            });
        }
    }
}
