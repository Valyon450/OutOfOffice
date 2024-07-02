using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Entities;

namespace BusinessLogic.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>()                
                .ReverseMap();

            CreateMap<LeaveRequest, LeaveRequestDTO>()
                .ReverseMap();

            CreateMap<ApprovalRequest, ApprovalRequestDTO>()
                .ReverseMap();

            CreateMap<Project, ProjectDTO>()
                .ReverseMap();
        }
    }
}
