using System.ComponentModel;

namespace DevTask.Services.Enums
{
    public enum TaskState
    {
        [Description("InProgress")]
        InProgress,
        [Description("Paused")]
        Paused,
        [Description("Completed")]
        Completed
    }
}