using AcademyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyApp.Domain.Interfaces
{
    public interface IExcelExportService
    {
        byte[] ExportCourses(IEnumerable<Course> courses);
        byte[] ExportStudents(IEnumerable<Student> students, Course course);
    }
}
