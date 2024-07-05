namespace BusinessLogic.Requests
{
    public class CreateOrUpdateApprovalRequest
    {
        public int ApproverId { get; set; } = default;
        public int LeaveRequestId { get; set; } = default;
        public string? Status { get; set; } = default;
        public string? Comment { get; set; } = default;
    }
}
