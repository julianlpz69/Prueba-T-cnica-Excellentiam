using AcademyApp.Application.DTOs;
using AcademyApp.Application.Interfaces;
using AcademyApp.Application.Pagination;
using AcademyApp.Domain.Entities;
using AcademyApp.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AcademyApp.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _repo;
        private readonly IRepository<Course> _courseRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<StudentDto>? _validator;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IRepository<Student> repo,
            IRepository<Course> courseRepo,
            IMapper mapper,
            ILogger<StudentService> logger,
            IValidator<StudentDto>? validator = null)
        {
            _repo = repo;
            _courseRepo = courseRepo;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<PagedResult<StudentDto>> GetAsync(QueryParams qp)
        {
            _logger.LogInformation("Fetching students with search '{Search}', page {Page}, pageSize {PageSize}", qp.Search, qp.Page, qp.PageSize);

            var filter = string.IsNullOrWhiteSpace(qp.Search)
                ? null
                : (System.Linq.Expressions.Expression<Func<Student, bool>>)(s =>
                      s.FullName.Contains(qp.Search));

            var sortBy = ResolveStudentSort(qp.SortBy);

            var items = await _repo.ListAsync(qp.Page, qp.PageSize, sortBy, qp.Desc, filter);
            var total = await _repo.CountAsync(filter);
            var dtos = _mapper.Map<IReadOnlyList<StudentDto>>(items);

            _logger.LogInformation("Found {Count} students", total);
            return new PagedResult<StudentDto>(dtos, total, qp.Page, qp.PageSize);
        }

        public async Task<PagedResult<StudentDto>> GetByCourseAsync(int courseId, QueryParams qp)
        {
            _logger.LogInformation("Fetching students for course {CourseId}, search '{Search}'", courseId, qp.Search);

            var filter = string.IsNullOrWhiteSpace(qp.Search)
                ? (System.Linq.Expressions.Expression<Func<Student, bool>>)(s => s.CourseId == courseId)
                : (System.Linq.Expressions.Expression<Func<Student, bool>>)(s =>
                      s.CourseId == courseId && s.FullName.Contains(qp.Search));

            var sortBy = ResolveStudentSort(qp.SortBy);

            var items = await _repo.ListAsync(qp.Page, qp.PageSize, sortBy, qp.Desc, filter);
            var total = await _repo.CountAsync(filter);
            var dtos = _mapper.Map<IReadOnlyList<StudentDto>>(items);

            _logger.LogInformation("Found {Count} students in course {CourseId}", total, courseId);
            return new PagedResult<StudentDto>(dtos, total, qp.Page, qp.PageSize);
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching student with id {Id}", id);
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<StudentDto>(entity);
        }

        public async Task<int> CreateAsync(StudentDto dto)
        {
            _logger.LogInformation("Creating student {FullName}", dto.FullName);

            if (_validator is not null)
                await _validator.ValidateAndThrowAsync(dto);

            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course is null)
            {
                _logger.LogWarning("Course {CourseId} not found while creating student", dto.CourseId);
                throw new KeyNotFoundException($"Course {dto.CourseId} not found.");
            }

            var entity = _mapper.Map<Student>(dto);
            entity.Id = 0;
            entity.CreatedAtUtc = DateTime.UtcNow;

            var created = await _repo.AddAsync(entity);
            _logger.LogInformation("Student {FullName} created with id {Id}", dto.FullName, created.Id);

            return created.Id;
        }

        public async Task UpdateAsync(int id, StudentDto dto)
        {
            _logger.LogInformation("Updating student {Id}", id);

            if (_validator is not null)
                await _validator.ValidateAndThrowAsync(dto);

            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Student {id} not found.");

            if (dto.CourseId != entity.CourseId)
            {
                var targetCourse = await _courseRepo.GetByIdAsync(dto.CourseId);
                if (targetCourse is null)
                {
                    _logger.LogWarning("Course {CourseId} not found when updating student {Id}", dto.CourseId, id);
                    throw new KeyNotFoundException($"Course {dto.CourseId} not found.");
                }
            }

            entity.FullName = dto.FullName;
            entity.Age = dto.Age;
            entity.Status = (Domain.Enums.StudentStatus)dto.Status;
            entity.EnrollmentDate = dto.EnrollmentDate;
            entity.CourseId = dto.CourseId;
            entity.UpdatedAtUtc = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            _logger.LogInformation("Student {Id} updated successfully", id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting student {Id}", id);

            var entity = await _repo.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Student {id} not found.");

            await _repo.DeleteAsync(entity);
            _logger.LogInformation("Student {Id} deleted", id);
        }

        private static string? ResolveStudentSort(string? sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy)) return null;

            switch (sortBy.Trim().ToLowerInvariant())
            {
                case "fullname": return nameof(Student.FullName);
                case "age": return nameof(Student.Age);
                case "status": return nameof(Student.Status);
                case "enrollmentdate": return nameof(Student.EnrollmentDate);
                case "createdatutc": return nameof(Student.CreatedAtUtc);
                default: return null;
            }
        }
    }
}
