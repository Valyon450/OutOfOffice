namespace BusinessLogic.DTOs
{
    public class LeaveRequestDTO
    {
        public int Id { get; set; }
        public required int EmployeeId { get; set; }
        public required string AbsenceReason { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public string? Comment { get; set; }
        public required string Status { get; set; }
    }
}
