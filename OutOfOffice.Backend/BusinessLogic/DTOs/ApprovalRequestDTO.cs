namespace BusinessLogic.DTOs
{
    public class ApprovalRequestDTO
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int LeaveRequestId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

        public EmployeeDTO Approver { get; set; }
        public LeaveRequestDTO LeaveRequest { get; set; }
    }
}
