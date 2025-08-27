using AcademyApp.Application.DTOs;
using AcademyApp.Application.Pagination;


namespace AcademyApp.Application.Interfaces
{
    public interface IStudentService
    {
        Task<PagedResult<StudentDto>> GetAsync(QueryParams qp);
        Task<PagedResult<StudentDto>> GetByCourseAsync(int courseId, QueryParams qp);
        Task<StudentDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(StudentDto dto);
        Task UpdateAsync(int id, StudentDto dto);
        Task DeleteAsync(int id);
    }
}
