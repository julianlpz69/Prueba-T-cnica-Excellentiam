using AcademyApp.Application.Interfaces;
using AcademyApp.Application.Mapping;
using AcademyApp.Application.Services;
using AcademyApp.Application.Validation;
using AcademyApp.Domain.Interfaces;
using AcademyApp.Infrastructure.Persistence;
using AcademyApp.Infrastructure.Repositories;
using AcademyApp.Infrastructure.Services;
using AcademyApp.Web.Components;
using AcademyApp.Web.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AcademyDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CourseValidator>();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var problem = Results.Problem(
            title: "Unexpected error",
            detail: ex?.Message,
            statusCode: StatusCodes.Status500InternalServerError);
        await problem.ExecuteAsync(context);
    });
});

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

MinimalApi.Map(app);

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
