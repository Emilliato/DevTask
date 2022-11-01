using DevTask.Services.Enums;
using SQLite;

namespace DevTask.Data
{
    public class DevTaskDto
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int TaskId { get; set; }

        [Column("name")]
        public string TaskName { get; set; }

        [Column("description")]
        public string TaskDescription { get; set; }

        [Column("state")]
        public TaskState State { get; set; }

        [Column("completed")]
        public string Completed { get; set; }

        [Column("estimate")]
        public string Estimate { get; set; }
    }
}