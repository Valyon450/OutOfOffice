using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityTypeConfigurations
{
    public class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
    {
        public void Configure(EntityTypeBuilder<ApprovalRequest> modelBuilder)
        {
            // ApprovalRequest - Approver relationship
            modelBuilder
                .HasOne(ar => ar.Approver)
                .WithMany()
                .HasForeignKey(ar => ar.ApproverID);

            // ApprovalRequest - LeaveRequest relationship
            modelBuilder
                .HasOne(ar => ar.LeaveRequest)
                .WithMany()
                .HasForeignKey(ar => ar.LeaveRequestID);
        }
    }
}
