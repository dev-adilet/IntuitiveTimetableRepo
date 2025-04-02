using IntuitiveTimetable.Dialogs;
using IntuitiveTimetable.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace IntuitiveTimetable.Pages
{
    public partial class Timetable
    {
        public bool IsTaskDialogVisible { get; set; }
        public bool IsOptionsMenuVisible { get; set; }
        public int SelectedRowIndex { get; set; } = -1;
        public TimetableEntry ?selectedTimetableEntry { get; set; }
        public List<TimetableEntry> timetableEntries = new List<TimetableEntry>
        {
            new TimetableEntry
            {
                StartTime = new TimeOnly(6,0),
                EndTime = new TimeOnly(6,30),
                TaskName = "Breakfast"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(6,30),
                EndTime = new TimeOnly(7,00),
                TaskName = "Gym"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(7,00),
                EndTime = new TimeOnly(7,30),
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
        public void OptionsMenuClicked(int index)
        {
            IsOptionsMenuVisible = true;
            SelectedRowIndex = index;
        }

        public void DeleteRow(int index)
        {
            timetableEntries.RemoveAt(index);
        }

        public void CloseOptions()
        {
            SelectedRowIndex = -1;
        }
    }
}
