using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevTask.Data
{
    public class DatabaseManager
    {
        private readonly SQLiteAsyncConnection _conn;

        public DatabaseManager()
        {
            try
            {
                var applicationFolderPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tasker");

                System.IO.Directory.CreateDirectory(applicationFolderPath);

                var databaseFileName = System.IO.Path.Combine(applicationFolderPath, "tasker.db");

                _conn = new SQLiteAsyncConnection(databaseFileName);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CreateTableResult> CreateTables()
        {
           return await _conn.CreateTableAsync<DevTaskDto>();
        }

        public async Task<IEnumerable<DevTaskDto>> GetTasks() => await _conn.Table<DevTaskDto>().ToListAsync();

        public async Task<int> CreateTask(DevTaskDto task)
        {
          return await  _conn.InsertAsync(task);
           
        }

        public async Task UpdateTask(DevTaskDto task)
        {
           await _conn.UpdateAsync(task);
        }

        public async Task DeleteTask(int taskId)
        {
           await _conn.DeleteAsync<DevTaskDto>(taskId);
        }
    }
}