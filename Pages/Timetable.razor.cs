using IntuitiveTimetable.Dialogs;
using IntuitiveTimetable.Models;
using System.ComponentModel;

namespace IntuitiveTimetable.Pages
{
    public partial class Timetable
    {
        public bool IsTaskDialogVisible { get; set; }
        public TimetableEntry selectedTimetableEntry { get; set; }
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

        public void AddRowButtonPressed()
        {
            IsTaskDialogVisible = true;
        }

        public void IsVisibleChanged(bool e)
        {
            IsTaskDialogVisible = e;
        }

        public void SaveRow(TaskData taskData)
        {
            var newRow = new TimetableEntry
            {
                StartTime = taskData.StartTime,
                EndTime = taskData.EndTime,
                TaskName = taskData.TaskName
            };

            timetableEntries.Add(newRow);
            closeAddRowDialog();
        }

        public void closeAddRowDialog()
        {
            IsTaskDialogVisible = false;
        }
    }
}
