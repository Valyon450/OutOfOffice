namespace DataAccess.Entities
{
    public class ApprovalRequest
    {
        public int ID { get; set; }
        public int ApproverID { get; set; }
        public int LeaveRequestID { get; set; }
        public string Status { get; set; } = "New";
        public string Comment { get; set; }

        public Employee Approver { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
    }
}
