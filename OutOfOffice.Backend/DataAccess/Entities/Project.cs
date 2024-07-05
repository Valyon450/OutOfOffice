namespace DataAccess.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public required string ProjectType { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime? EndDate { get; set; }
        public required int ProjectManagerId { get; set; }
        public string? Comment { get; set; }
        public required string Status { get; set; }

        public required Employee ProjectManager { get; set; }
    }
}
