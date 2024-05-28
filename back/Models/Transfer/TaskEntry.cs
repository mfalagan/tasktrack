namespace back.Models.Transfer
{
    public class TaskEntry
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly? DueDate { get; set; }

        public TaskEntry(Models.Internal.Task task)
        {
            this.Id = task.Id;
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
        public TaskEntry(TaskData task)
        {
            this.Id = null;
            this.Name = task.Name;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
        }
    }
}
