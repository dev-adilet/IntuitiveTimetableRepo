namespace IntuitiveTimetable.Models
{
    public class TaskData
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? TaskName { get; set; }
    }
}
