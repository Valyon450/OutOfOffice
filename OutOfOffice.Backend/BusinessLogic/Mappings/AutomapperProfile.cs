using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Requests;
using DataAccess.Entities;

namespace BusinessLogic.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ReverseMap();
            CreateMap<CreateOrUpdateEmployee, Employee>();

            CreateMap<LeaveRequest, LeaveRequestDTO>()
                .ReverseMap();
            CreateMap<CreateOrUpdateLeaveRequest, LeaveRequest>();

            CreateMap<ApprovalRequest, ApprovalRequestDTO>()
                .ReverseMap();
            CreateMap<CreateOrUpdateApprovalRequest, ApprovalRequest>();

            CreateMap<Project, ProjectDTO>()
                .ReverseMap();
            CreateMap<CreateOrUpdateProject, Project>();
        }
    }
}
