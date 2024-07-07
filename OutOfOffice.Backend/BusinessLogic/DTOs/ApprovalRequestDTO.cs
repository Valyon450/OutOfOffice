namespace BusinessLogic.DTOs
{
    public class ApprovalRequestDTO
    {
        public int Id { get; set; }
        public required int ApproverId { get; set; }
        public required int LeaveRequestId { get; set; }
        public required string Status { get; set; }
        public string? Comment { get; set; }
    }
}
