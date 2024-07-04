using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Mappings;

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

            return services;
        }
    }
}
