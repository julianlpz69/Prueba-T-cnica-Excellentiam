using AcademyApp.Application.DTOs;
using AcademyApp.Application.Pagination;


namespace AcademyApp.Application.Interfaces
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>> GetAsync(QueryParams qp);
        Task<CourseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CourseDto dto);
        Task UpdateAsync(int id, CourseDto dto);
        Task DeleteAsync(int id);
    }
}
