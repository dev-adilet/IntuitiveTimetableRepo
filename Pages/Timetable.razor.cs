using IntuitiveTimetable.Models;
using System.ComponentModel;

namespace IntuitiveTimetable.Pages
{
    public partial class Timetable
    {

        public List<TimetableEntry> timetableEntries = new List<TimetableEntry>
        {
            new TimetableEntry
            {
                StartTime = "6:00 AM",
                EndTime = "6:30 AM",
                TaskName = "Breakfast"
            },
            new TimetableEntry
            {
                StartTime = "6:30 AM",
                EndTime = "7:00 AM",
                TaskName = "Gym"
            },
            new TimetableEntry
            {
                StartTime = "7:00 AM",
                EndTime = "7:30 AM",
                TaskName = "Work Prep"
            }
        };
    }
}
