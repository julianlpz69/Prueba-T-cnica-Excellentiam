using AcademyApp.Application.DTOs;
using FluentValidation;

namespace AcademyApp.Application.Validation
{
    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(150).WithMessage("Full name must not exceed 150 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(10, 120)
                .WithMessage("Age must be between 10 and 120.");

            RuleFor(x => x.EnrollmentDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Enrollment date cannot be in the future.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("A valid course must be selected.");
        }
    }
}
