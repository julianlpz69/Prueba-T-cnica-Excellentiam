using AcademyApp.Application.DTOs;
using AcademyApp.Domain.Entities;
using AutoMapper;

namespace AcademyApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}
