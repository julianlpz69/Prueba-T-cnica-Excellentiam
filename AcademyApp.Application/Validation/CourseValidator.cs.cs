

using AcademyApp.Application.DTOs;
using FluentValidation;

namespace AcademyApp.Application.Validation
{
    public class CourseValidator : AbstractValidator<CourseDto>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(150).WithMessage("Course name cannot exceed 150 characters");

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 30).WithMessage("Credits must be between 1 and 30");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Course name is required");

            RuleFor(x => x.StartDate)
                .LessThan(DateTime.UtcNow.AddYears(5)).WithMessage("Start date cannot be more than 5 years in the future");

        }
    }
}
