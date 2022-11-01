using DevTask.Services.ViewModels;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevTask.Services.Interfaces
{
    public interface IDevTaskManager
    {
        Task<CreateTableResult> CreateTables();
        Task<IEnumerable<TaskViewModel>> GetTasks();
        TaskViewModel GetTaskDetails(int taskId);

        Task AddTask(TaskViewModel task);
        Task UpdateTask(TaskViewModel task);

        void CompleteTask(TaskViewModel task);

        void SaveState(TaskViewModel task);
        Task DeleteTask(int taskId);
    }
}