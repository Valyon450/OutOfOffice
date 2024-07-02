namespace DataAccess.Entities
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int LeaveRequestId { get; set; }
        public string Status { get; set; } = "New";
        public string Comment { get; set; }

        public Employee Approver { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
    }
}
