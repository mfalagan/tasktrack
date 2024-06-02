namespace back.Models.Internal
{
    public enum PriorityLevel
    {
        High,
        Medium,
        Low
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly DueDate { get; set; }
        public PriorityLevel Priority { get; set; }
        public int UserId { get; set; }
        public User Owner { get; set; }
    }
}