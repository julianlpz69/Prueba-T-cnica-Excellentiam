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
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _repo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<CourseDto>? _validator;
        private readonly ILogger<CourseService> _logger;

        public CourseService(
            IRepository<Course> repo,
            IRepository<Student> studentRepo,
            IMapper mapper,
            ILogger<CourseService> logger,   
            IValidator<CourseDto>? validator = null)
        {
            _repo = repo;
            _studentRepo = studentRepo;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<PagedResult<CourseDto>> GetAsync(QueryParams qp)
        {
            _logger.LogInformation("Fetching courses. Page: {Page}, Size: {PageSize}, SortBy: {SortBy}, Desc: {Desc}, Search: {Search}",
                qp.Page, qp.PageSize, qp.SortBy, qp.Desc, qp.Search);

            var filter = string.IsNullOrWhiteSpace(qp.Search)
                ? null
                : (System.Linq.Expressions.Expression<Func<Course, bool>>)(c =>
                      c.Name.Contains(qp.Search));

            var sortBy = ResolveCourseSort(qp.SortBy);

            var items = await _repo.ListAsync(qp.Page, qp.PageSize, sortBy, qp.Desc, filter);
            var total = await _repo.CountAsync(filter);

            _logger.LogInformation("Fetched {Count} courses out of {Total}", items.Count, total);

            var dtos = _mapper.Map<IReadOnlyList<CourseDto>>(items);
            return new PagedResult<CourseDto>(dtos, total, qp.Page, qp.PageSize);
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching course with Id={Id}", id);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Course with Id={Id} not found", id);
                return null;
            }

            _logger.LogInformation("Course with Id={Id} found: {Name}", id, entity.Name);
            return _mapper.Map<CourseDto>(entity);
        }

        public async Task<int> CreateAsync(CourseDto dto)
        {
            _logger.LogInformation("Creating course: {@Course}", dto);

            if (_validator is not null)
                await _validator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Course>(dto);
            entity.Id = 0;
            entity.CreatedAtUtc = DateTime.UtcNow;

            var created = await _repo.AddAsync(entity);

            _logger.LogInformation("Course created successfully with Id={Id}", created.Id);

            return created.Id;
        }

        public async Task UpdateAsync(int id, CourseDto dto)
        {
            _logger.LogInformation("Updating course with Id={Id}", id);

            if (_validator is not null)
                await _validator.ValidateAndThrowAsync(dto);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogError("Cannot update. Course with Id={Id} not found", id);
                throw new KeyNotFoundException($"Course {id} not found.");
            }

            entity.Name = dto.Name;
            entity.Credits = dto.Credits;
            entity.Category = (Domain.Enums.CourseCategory)dto.Category;
            entity.StartDate = dto.StartDate;
            entity.UpdatedAtUtc = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);

            _logger.LogInformation("Course with Id={Id} updated successfully", id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Attempting to delete course with Id={Id}", id);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogError("Cannot delete. Course with Id={Id} not found", id);
                throw new KeyNotFoundException($"Course {id} not found.");
            }

            var count = await _studentRepo.CountAsync(s => s.CourseId == id);
            if (count > 0)
            {
                _logger.LogWarning("Cannot delete course with Id={Id}. It has {Count} enrolled students.", id, count);
                throw new InvalidOperationException(
                    $"Course {id} cannot be deleted because it has enrolled students. Please remove the students first.");
            }

            await _repo.DeleteAsync(entity);

            _logger.LogInformation("Course with Id={Id} deleted successfully", id);
        }

        private static string? ResolveCourseSort(string? sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy)) return null;

            return sortBy.Trim().ToLowerInvariant() switch
            {
                "name" => nameof(Course.Name),
                "credits" => nameof(Course.Credits),
                "category" => nameof(Course.Category),
                "startdate" => nameof(Course.StartDate),
                "createdatutc" => nameof(Course.CreatedAtUtc),
                _ => null
            };
        }
    }
}
