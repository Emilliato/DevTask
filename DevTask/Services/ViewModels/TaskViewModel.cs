using DevTask.Services.Enums;

namespace DevTask.Services.ViewModels
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public TaskState State { get; set; }
        public string Estimate { get; set; }
        public string Completed { get; set; }
    }
}