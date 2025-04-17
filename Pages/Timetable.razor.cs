using IntuitiveTimetable.Dialogs;
using IntuitiveTimetable.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace IntuitiveTimetable.Pages
{
    public partial class Timetable
    {
        public bool IsOptionsMenuVisible { get; set; }
        public int SelectedRowIndex { get; set; } = -1;
        public bool IsAddTaskDialogVisible { get; set; }
        public bool IsEditTaskDialogVisible { get; set; }
        public TimetableEntry ?selectedTimetableEntry { get; set; }
        public int selectedIndex { get; set; }
        public List<TimetableEntry> timetableEntries = new List<TimetableEntry>
        {
            new TimetableEntry
            {
                StartTime = new TimeOnly(6,0),
                EndTime = new TimeOnly(6,30),
                TaskName = "Row1"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(6,30),
                EndTime = new TimeOnly(7,00),
                TaskName = "Row2"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(7,00),
                EndTime = new TimeOnly(7,30),
                TaskName = "Row3"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(7,30),
                EndTime = new TimeOnly(8,00),
                TaskName = "Row4"
            },
            new TimetableEntry
            {
                StartTime = new TimeOnly(8,00),
                EndTime = new TimeOnly(8,15),
                TaskName = "Row5"
            }
        };

        public void AddRowButtonPressed()
        {
            IsAddTaskDialogVisible = true;
        }

        public void AddTaskDialogVisChanged(bool e)
        {
            IsAddTaskDialogVisible = e;
        }

        public void EditTaskDialogVisChanged(bool e)
        {
            IsAddTaskDialogVisible = e;
        }

        public void UpdateTask(TaskData taskData, int index)
        {
            //
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
            IsAddTaskDialogVisible = false;
        }
        public void OptionsMenuClicked(int index)
        {
            IsOptionsMenuVisible = true;
            SelectedRowIndex = index;
        }

        public void DeleteRow(int index)
        {
            List<TimetableEntry> timetableEntries2 = timetableEntries
                .Select(entry => entry.Clone())
                .ToList();

            for (int i = 0; i < timetableEntries2.Count - (index + 1); i++)
            {

                if (i == 0)
                {
                    var temp = timetableEntries2[index + 1];
                    var duration = timetableEntries2[index + 1].EndTime - timetableEntries2[index + 1].StartTime;
                    timetableEntries2[index + 1].StartTime = timetableEntries2[index - 1].EndTime;
                    timetableEntries2[index + 1].EndTime = timetableEntries2[index + 1].StartTime.AddMinutes(duration.Minutes);
                } 
                else
                {
                    var rowBeingChanged = timetableEntries2[index + 1 + i];
                    var temp = timetableEntries2[index - 1 + i];
                    var duration = timetableEntries2[index + 1 + i].EndTime - timetableEntries2[index + 1 + i].StartTime;
                    timetableEntries2[index + 1 + i].StartTime = timetableEntries2[index + i].EndTime;
                    timetableEntries2[index + 1 + i].EndTime = timetableEntries2[index + 1 + i].StartTime.AddMinutes(duration.Minutes);
                }
            }

            timetableEntries2.RemoveAt(index);

            timetableEntries = timetableEntries2;

            CloseOptions();
        }

        public void CloseOptions()
        {
            SelectedRowIndex = -1;
        }
    }
}
