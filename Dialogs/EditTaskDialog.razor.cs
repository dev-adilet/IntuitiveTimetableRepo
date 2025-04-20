using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IntuitiveTimetable.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace IntuitiveTimetable.Dialogs // Replace with your actual namespace
{
    public partial class EditTaskDialog : ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        [Parameter]
        public EventCallback<TaskData> TaskUpdated { get; set; }

        [Parameter]
        public TimeOnly ?StartTime { get; set; }

        [Parameter]
        public TimeOnly ?EndTime { get; set; }

        [Parameter]
        public string ?TaskName { get; set; }

        [Parameter]
        public string? ValidationErrorMessage { get; set; }
        [Parameter]
        public bool? IsItFirstRow { get; set; }
        public int selectedEditRowIndex { get; set; }

        protected override void OnInitialized()
        {
            EndTime = StartTime;
        }

        protected async Task CloseModal()
        {
            await VisibleChanged.InvokeAsync(false);
            this.TaskName = string.Empty;
        }

        protected async Task Cancel()
        {
            await CloseModal();
        }

        protected async Task Update()
        {
            var taskData = new TaskData
            {
                StartTime = (TimeOnly)StartTime,
                EndTime = (TimeOnly)EndTime,
                TaskName = String.IsNullOrEmpty(TaskName) ? "" : TaskName
            };

            await TaskUpdated.InvokeAsync(taskData);
        }
    }
}
