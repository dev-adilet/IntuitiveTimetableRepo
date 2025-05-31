using IntuitiveTimetable.Dialogs;
using IntuitiveTimetable.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.ComponentModel;
using System.Text.Json;

namespace IntuitiveTimetable.Pages
{
    public partial class Timetable
    {
        [Inject]
        private IJSRuntime JS { get; set; }
        public bool IsOptionsMenuVisible { get; set; }
        public int MenuOptionsRowIndex { get; set; } = -1;
        public bool IsAddTaskDialogVisible { get; set; }
        public bool IsEditTaskDialogVisible { get; set; }
        public bool IsItFirstTask { get; set; } = false;
        public int IndexWhereToAdd { get; set; } = -1;

        public TimetableEntry? selectedTimetableEntry { get; set; } = new TimetableEntry { };
        public int selectedEditRowIndex { get; set; }

        public string EditTaskValidationErrorMessage { get; set; } = string.Empty;
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
            AddTaskDialogVisChanged(true);
        }

        public void AddRowButtonPressed(int index)
        {
            AddTaskDialogVisChanged(true, index);
        }

        public void AddTaskDialogVisChanged(bool e)
        {
            IsAddTaskDialogVisible = e;
        }

        public void AddTaskDialogVisChanged(bool e, int index)
        {
            IsAddTaskDialogVisible = e;
            IndexWhereToAdd = index;
        }

        public void EditTaskDialogVisChanged(bool e)
        {
            IsEditTaskDialogVisible = e;
            if (e == false)
            {
                EditTaskValidationErrorMessage = string.Empty;
                IsItFirstTask = false;
            }
        }

        public void UpdateRowDetails(TaskData taskData)
        {
            if (timetableEntries.Count == selectedEditRowIndex + 1)
            {
                timetableEntries[selectedEditRowIndex].StartTime = taskData.StartTime;
                timetableEntries[selectedEditRowIndex].EndTime = taskData.EndTime;
                timetableEntries[selectedEditRowIndex].TaskName = taskData.TaskName;
                if (timetableEntries.Count != selectedEditRowIndex + 1)
                {
                    timetableEntries[selectedEditRowIndex + 1].StartTime = taskData.EndTime;
                }
                CloseEditRowDialog();
            } 
            else
            {
                if (taskData.EndTime > timetableEntries[selectedEditRowIndex + 1].StartTime
                || taskData.StartTime > timetableEntries[selectedEditRowIndex + 1].StartTime)
                {
                    EditTaskValidationErrorMessage =
                        "\"Start time\" and/or \"End time\" " +
                        "must not exceed the next task \"Start time\"";
                }
                else
                {
                    timetableEntries[selectedEditRowIndex].StartTime = taskData.StartTime;
                    timetableEntries[selectedEditRowIndex].EndTime = taskData.EndTime;
                    timetableEntries[selectedEditRowIndex].TaskName = taskData.TaskName;
                    if (timetableEntries.Count != selectedEditRowIndex + 1)
                    {
                        timetableEntries[selectedEditRowIndex + 1].StartTime = taskData.EndTime;
                    }
                    CloseEditRowDialog();
                }
            }
        }

        public void UpdateRow(int index)
        {
            if (index == 0)
            {
                IsItFirstTask = true;
            }
            selectedEditRowIndex = index;
            selectedTimetableEntry = timetableEntries[index];
            IsEditTaskDialogVisible = true;
        }

        public void AddRow(TaskData taskData)
        {
            var newRow = new TimetableEntry
            {
                StartTime = taskData.StartTime,
                EndTime = taskData.EndTime,
                TaskName = taskData.TaskName
            }; 

            timetableEntries.Add(newRow);
            CloseAddRowDialog();
        }

        public void AddRowAtIndex(TaskData taskData)
        {
            var newRow = new TimetableEntry
            {
                StartTime = taskData.StartTime,
                EndTime = taskData.EndTime,
                TaskName = taskData.TaskName
            };
            timetableEntries.Insert(IndexWhereToAdd, newRow);
            AdjustOtherRowsAfterAddAtIndex(IndexWhereToAdd);
            CloseAddRowDialog();
        }

        protected void AdjustOtherRowsAfterAddAtIndex(int newRowIndex)
        {
            for (int i = newRowIndex + 1; i < timetableEntries.Count; i++)
            {
                var duration = timetableEntries[i].EndTime - timetableEntries[i].StartTime;
                timetableEntries[i].StartTime = timetableEntries[i - 1].EndTime;
                timetableEntries[i].EndTime = timetableEntries[i].StartTime.AddMinutes(duration.Minutes);
            }
        }

        public void CloseAddRowDialog()
        {
            AddTaskDialogVisChanged(false);
        }

        public void CloseEditRowDialog()
        {
            EditTaskDialogVisChanged(false);
        }
        public void OptionsMenuClicked(int index)
        {
            IsOptionsMenuVisible = true;
            MenuOptionsRowIndex = index;
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
            MenuOptionsRowIndex = -1;
        }

        public async Task ExportAsync()
        {
            string json = JsonSerializer.Serialize(timetableEntries);
            await JS.InvokeVoidAsync("downloadFile", DateTime.Today.ToShortDateString().Replace("/", "-") + ".json", json);
        }

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            using var stream = file.OpenReadStream(1024 * 1024);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var loadedEntries = JsonSerializer.Deserialize<List<TimetableEntry>>(json);
            if (loadedEntries != null)
            {
                timetableEntries = loadedEntries;
            }
        }
    }
}
