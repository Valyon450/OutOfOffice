using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IOutOfOfficeDbContext
    {
        DbSet<Employee> Employees { get; }
        DbSet<LeaveRequest> LeaveRequests { get; }
        DbSet<ApprovalRequest> ApprovalRequests { get; }
        DbSet<Project> Projects { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
