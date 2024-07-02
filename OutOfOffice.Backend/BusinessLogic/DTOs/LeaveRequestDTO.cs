namespace BusinessLogic.DTOs
{
    public class LeaveRequestDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string AbsenceReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }
}
