using back.Models;
using back.Models.Internal;
using back.Models.Transfer;
using Microsoft.EntityFrameworkCore;

namespace back.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetUserEvents(int userId);
        Task<List<Event>> GetNextEvents(int userId, int n);
        Task<Event?> GetEvent(int userId, int eventId);
        Task<Event> AddEvent(int userId, EventData eventData);
        Task<Event?> UpdateEvent(int userId, int eventId, EventData eventData);
        Task<Event?> DeleteEvent(int userId, int eventId);
    }

    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetUserEvents(int userId)
        {
            return await _context.Events
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Event>> GetNextEvents(int userId, int n)
        {
            return await _context.Events
                .Where(e => e.UserId == userId && e.DueDate >= DateOnly.FromDateTime(DateTime.UtcNow))
                .OrderBy(e => e.DueDate)
                .Take(n)
                .ToListAsync();
        }

        public async Task<Event?> GetEvent(int userId, int eventId)
        {
            return await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.UserId == userId);
        }

        public async Task<Event> AddEvent(int userId, EventData eventData)
        {
            var newEvent = new Event
            {
                Title = eventData.Title ?? throw new ArgumentNullException("Title must be provided"),
                Description = eventData.Description,
                DueDate = eventData.DueDate,
                Priority = eventData.Priority,
                UserId = userId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return newEvent;
        }

        public async Task<Event?> UpdateEvent(int userId, int eventId, EventData eventData)
        {
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.UserId == userId);

            if (eventEntity == null)
            {
                return null;
            }

            eventEntity.Title = eventData.Title ?? throw new ArgumentNullException("Title must be provided");
            eventEntity.Description = eventData.Description;
            eventEntity.DueDate = eventData.DueDate;
            eventEntity.Priority = eventData.Priority;

            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();

            return eventEntity;
        }

        public async Task<Event?> DeleteEvent(int userId, int eventId)
        {
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.UserId == userId);

            if (eventEntity == null)
            {
                return null;
            }

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();

            return eventEntity;
        }
    }
}
