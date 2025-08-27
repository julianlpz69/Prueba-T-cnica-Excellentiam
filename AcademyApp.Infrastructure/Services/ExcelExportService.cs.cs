using AcademyApp.Domain.Entities;
using AcademyApp.Domain.Interfaces;
using ClosedXML.Excel;

namespace AcademyApp.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportCourses(IEnumerable<Course> courses)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Courses");

            // Headers
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Credits";
            worksheet.Cell(1, 4).Value = "Category";
            worksheet.Cell(1, 5).Value = "Start Date";

            int row = 2;
            foreach (var c in courses)
            {
                worksheet.Cell(row, 1).Value = c.Id;
                worksheet.Cell(row, 2).Value = c.Name;
                worksheet.Cell(row, 3).Value = c.Credits;
                worksheet.Cell(row, 4).Value = c.Category.ToString();
                worksheet.Cell(row, 5).Value = c.StartDate.ToString("yyyy-MM-dd");
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportStudents(IEnumerable<Student> students, Course course)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Students");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Full Name";
            worksheet.Cell(1, 3).Value = "Age";
            worksheet.Cell(1, 4).Value = "Status";
            worksheet.Cell(1, 5).Value = "Enrollment Date";
            worksheet.Cell(1, 6).Value = "Course";

            int row = 2;
            foreach (var s in students)
            {
                worksheet.Cell(row, 1).Value = s.Id;
                worksheet.Cell(row, 2).Value = s.FullName;
                worksheet.Cell(row, 3).Value = s.Age;
                worksheet.Cell(row, 4).Value = s.Status.ToString();
                worksheet.Cell(row, 5).Value = s.EnrollmentDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 6).Value = course.Name;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
