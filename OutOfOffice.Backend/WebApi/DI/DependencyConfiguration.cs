using BusinessLogic.Services;
using DataAccess.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Mappings;
using BusinessLogic.Validators;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Requests;
using BusinessLogic.ValidationServices.Interfaces;
using BusinessLogic.ValidationServices;
using FluentValidation;

namespace WebApi.DI
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection ConfigureDependency(this IServiceCollection services, IConfiguration configuration)
        {
            // Database context
            services.AddDbContext<IOutOfOfficeDbContext, OutOfOfficeDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(typeof(AutomapperProfile));

            // Services
            services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            services.AddScoped<IProjectService, ProjectService>();

            // Validators
            services.AddScoped<IValidator<CreateOrUpdateEmployee>, EmployeeValidator>();
            services.AddScoped<IValidator<CreateOrUpdateApprovalRequest>, ApprovalRequestValidator>();
            services.AddScoped<IValidator<CreateOrUpdateLeaveRequest>, LeaveRequestValidator>();
            services.AddScoped<IValidator<CreateOrUpdateProject>, ProjectValidator>();

            // Validation services
            services.AddScoped<IEmployeeValidationService, EmployeeValidationService>();
            services.AddScoped<IApprovalRequestValidationService, ApprovalRequestValidationService>();
            services.AddScoped<ILeaveRequestValidationService, LeaveRequestValidationService>();
            services.AddScoped<IProjectValidationService, ProjectValidationService>();

            return services;
        }
    }
}
