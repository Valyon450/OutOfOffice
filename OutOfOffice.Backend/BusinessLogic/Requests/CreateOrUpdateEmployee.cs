namespace BusinessLogic.Requests
{
    public class CreateOrUpdateEmployee
    {
        public string? FullName { get; set; } = default;
        public string? Subdivision { get; set; } = default;
        public string? Position { get; set; } = default;
        public string? Status { get; set; } = default;
        public int PeoplePartnerId { get; set; } = default;
        public int OutOfOfficeBalance { get; set; } = default; 
    }
}
