using DataAccess.Entities;
using DataAccess.EntityTypeConfigurations;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutOfOfficeDbContext : DbContext, IOutOfOfficeDbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<LeaveRequest> LeaveRequest { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequest { get; set; }
        public DbSet<Project> Project { get; set; }

        public OutOfOfficeDbContext(DbContextOptions<OutOfOfficeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        }
    }

}
