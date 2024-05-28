namespace back.Models.Transfer
{
    public class TaskData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly? DueDate { get; set; }

        public TaskData(Models.Internal.Task task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
        public TaskData(TaskEntry task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
    }
}
