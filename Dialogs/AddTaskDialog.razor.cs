using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IntuitiveTimetable.Models;
using Microsoft.AspNetCore.Components;

namespace IntuitiveTimetable.Dialogs // Replace with your actual namespace
{
    public partial class AddTaskDialog : ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Parameter]
        public EventCallback<TaskData> OnSave { get; set; }

        [Parameter]
        public TimeOnly StartTime { get; set; }
        protected TimeOnly EndTime { get; set; } 
        protected string ?TaskName { get; set; }

        protected override void OnInitialized()
        {
            EndTime = StartTime;
        }

        protected async Task CloseModal()
        {
            await IsVisibleChanged.InvokeAsync(false);
            this.TaskName = string.Empty;
        }

        protected async Task Cancel()
        {
            await CloseModal();
        }

        protected async Task Save()
        {
            var taskData = new TaskData
            {
                StartTime = StartTime,
                EndTime = EndTime,
                TaskName = String.IsNullOrEmpty(TaskName) ? "" : TaskName
            };

            await OnSave.InvokeAsync(taskData);
            await CloseModal();
        }
    }

    // This class holds the task data; you could also place it in a separate file if desired.
    public class TaskData
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string ?TaskName { get; set; }
    }
}
