using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IOutOfOfficeDbContext
    {
        DbSet<Employee> Employee { get; }
        DbSet<LeaveRequest> LeaveRequest { get; }
        DbSet<ApprovalRequest> ApprovalRequest { get; }
        DbSet<Project> Project { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
