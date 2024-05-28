namespace back.Models.Internal
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateOnly? DueDate { get; set; }

        // public Task(Models.DTO.TaskEntry task)
        // {
        //     this.Name = task.Name;
        //     this.Description = task.Description;
        //     this.DueDate = (task.DueDate == null) ? null : task.DueDate.ToDateOnly();
        // }
        // public Task(Models.DTO.TaskData task)
        // {
        //     this.Name = task.Name;
        //     this.Description = task.Description;
        //     this.DueDate = (task.DueDate == null) ? null : task.DueDate.ToDateOnly();
        // }
        public Task() { }
    }
}
