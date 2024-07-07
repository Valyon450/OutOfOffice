namespace BusinessLogic.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Subdivision { get; set; }
        public required string Position { get; set; }
        public required string Status { get; set; }
        public int? PeoplePartnerId { get; set; }
        public int? ProjectId { get; set; }
        public required int OutOfOfficeBalance { get; set; }
        public byte[]? Photo { get; set; }
    }
}

