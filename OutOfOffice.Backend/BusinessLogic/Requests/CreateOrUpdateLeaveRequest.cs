namespace BusinessLogic.Requests
{
    public class CreateOrUpdateLeaveRequest
    {
        public int EmployeeId { get; set; } = default;
        public string? AbsenceReason { get; set; } = default;
        public DateTime StartDate { get; set; } = default;
        public DateTime EndDate { get; set; } = default;
        public string? Comment { get; set; } = default;
        public string? Status { get; set; } = default;
    }
}
