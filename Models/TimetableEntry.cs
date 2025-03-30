namespace IntuitiveTimetable.Models
{
    public class TimetableEntry
    {

        public TimeOnly StartTime { get; set; }  

        public TimeOnly EndTime { get; set; }

        public string ?TaskName { get; set; }
    }
}
