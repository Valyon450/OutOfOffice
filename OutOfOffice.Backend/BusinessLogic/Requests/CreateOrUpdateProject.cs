namespace BusinessLogic.Requests
{
    public class CreateOrUpdateProject
    {
        public string? ProjectType { get; set; } = default;
        public DateTime StartDate { get; set; } = default;
        public DateTime EndDate { get; set; } = default;
        public int ProjectManagerId { get; set; } = default;
        public string? Comment { get; set; } = default;
        public string? Status { get; set; } = default;
    }
}
