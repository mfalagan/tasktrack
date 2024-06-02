namespace back.Models.Transfer
{
    public class EventData
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly DueDate { get; set; }
        public Models.Internal.PriorityLevel Priority { get; set; }

        public EventData() { }

        public EventData(Models.Internal.Event eventEntity)
        {
            Title = eventEntity.Title;
            Description = eventEntity.Description;
            DueDate = eventEntity.DueDate;
            Priority = eventEntity.Priority;
        }
    }

    public class EventEntry : EventData
    {
        public int Id { get; set; }

        public EventEntry() { }

        public EventEntry(Models.Internal.Event eventEntity) : base(eventEntity)
        {
            Id = eventEntity.Id;
        }
    }
}
