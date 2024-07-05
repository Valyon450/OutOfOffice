using DataAccess.Entities;

namespace BusinessLogic.DTOs
{
    public class ApprovalRequestDTO
    {
        public int Id { get; set; }
        public required int ApproverId { get; set; }
        public required int LeaveRequestId { get; set; }
        public string Status { get; set; } = "New";
        public string? Comment { get; set; }

        public required Employee Approver { get; set; }
        public required LeaveRequest LeaveRequest { get; set; }
    }
}
