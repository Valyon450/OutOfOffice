using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutOfOfficeDbContext : DbContext
    {
        public OutOfOfficeDbContext(DbContextOptions<OutOfOfficeDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.PeoplePartner)
                .WithMany()
                .HasForeignKey(e => e.PeoplePartner);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany()
                .HasForeignKey(lr => lr.EmployeeID);

            modelBuilder.Entity<ApprovalRequest>()
                .HasOne(ar => ar.Approver)
                .WithMany()
                .HasForeignKey(ar => ar.ApproverID);

            modelBuilder.Entity<ApprovalRequest>()
                .HasOne(ar => ar.LeaveRequest)
                .WithMany()
                .HasForeignKey(ar => ar.LeaveRequestID);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany()
                .HasForeignKey(p => p.ProjectManagerID);
        }
    }

}
