namespace TimeManager.API.Models
{
    public class Timeregistration
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int EmployeeId { get; set; }
    }
}
