using DevTask.Data;
using DevTask.Services.Interfaces;
using DevTask.Services.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.Services.Implementations
{
    public class DevTaskManager : IDevTaskManager
    {
        private readonly DatabaseManager _db;

        public DevTaskManager()
        {
            _db = new DatabaseManager();
        }

        public async Task<CreateTableResult> CreateTables()
        {
            return await _db.CreateTables();
        }

        public async Task AddTask(TaskViewModel task)
        {
            var newTask = new DevTaskDto();
            newTask.TaskName = task.TaskName;
            newTask.TaskDescription = task.TaskDescription;
            newTask.Completed =  task.Completed;
            newTask.Estimate =  task.Estimate;
            newTask.State = Enums.TaskState.InProgress;

            await _db.CreateTask(newTask);
        }

        public void CompleteTask(TaskViewModel task)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteTask(int id)
        {
            await _db.DeleteTask(id);
        }

        public TaskViewModel GetTaskDetails(int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskViewModel>> GetTasks()
        {
            var tasks = await _db.GetTasks();

            return tasks.Select( t => new TaskViewModel()
            {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskDescription = t.TaskDescription,
                Estimate = t.Estimate,
                Completed = t.Completed,
                State = t.State,
            });
        }

        public void SaveState(TaskViewModel task)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTask(TaskViewModel task)
        {
            var newTask = new DevTaskDto();
            newTask.TaskId = task.TaskId;
            newTask.TaskName = task.TaskName;
            newTask.TaskDescription = task.TaskDescription;
            newTask.Completed = task.Completed;
            newTask.Estimate = task.Estimate;
            newTask.State = Enums.TaskState.InProgress;

            await _db.UpdateTask(newTask);
        }
    }
}