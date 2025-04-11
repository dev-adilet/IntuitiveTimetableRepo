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
            List<TimetableEntry> timetableEntries2 = timetableEntries
                .Select(entry => entry.Clone())
                .ToList();

            for (int i = 0; i < timetableEntries2.Count-2; i++)
            {
                //first cycle of the loop is to get index + 1 row start time set to index - 1 row endtime, as the index row is being deleted
                if (i == 0)
                {
                    if (index == timetableEntries.Count-1)
                    {
                        //do nothing
                    } 
                    else
                    {
                        var duration = timetableEntries2[index + 1].EndTime - timetableEntries2[index + 1].StartTime;
                        timetableEntries2[index + 1].StartTime = timetableEntries2[index - 1].EndTime;
                        timetableEntries2[index + 1].EndTime = timetableEntries2[index + 1].StartTime.AddMinutes(duration.Minutes);
                    }
                    
                }
                else
                {
                    var duration = timetableEntries2[i + 2].EndTime - timetableEntries2[i + 2].StartTime;
                    timetableEntries2[i + 2].StartTime = timetableEntries2[i + 1].EndTime;
                    timetableEntries2[i + 2].EndTime = timetableEntries2[i + 2].StartTime.AddMinutes(duration.Minutes);
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
