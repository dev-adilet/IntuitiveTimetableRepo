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
        public EventCallback<bool> VisibleChanged { get; set; }

        [Parameter]
        public EventCallback<TaskData> OnAddRow { get; set; }
        
        [Parameter]
        public EventCallback<TaskData> OnAddRowAtIndex { get; set; }

        [Parameter]
        public int IndexWhereToAdd { get; set; }

        [Parameter]
        public TimeOnly StartTime { get; set; }
        protected TimeOnly EndTime { get; set; } 
        protected string? TaskName { get; set; }

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

        protected async Task AddRow()
        {
            var taskData = new TaskData
            {
                StartTime = StartTime,
                EndTime = EndTime,
                TaskName = String.IsNullOrEmpty(TaskName) ? "" : TaskName
            };

            if (IndexWhereToAdd == -1)
            {
                await OnAddRow.InvokeAsync(taskData);
            } 
            else
            {
                await OnAddRowAtIndex.InvokeAsync(taskData);
            }

            await CloseModal();
        }
    }
}
