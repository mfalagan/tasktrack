using back.Models.Transfer;

namespace back.Models.Internal
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateOnly? DueDate { get; set; }

        public Task(TaskEntry task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
        public Task(TaskData task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
        public Task() { }
    }
}
