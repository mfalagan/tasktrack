using back.Models.Internal;
using back.Models.Transfer;
using back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace back.Controllers
{
    [Route("/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventController(IEventService eventService, IHttpContextAccessor httpContextAccessor)
        {
            _eventService = eventService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<EventEntry>>> GetEvents()
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var events = await _eventService.GetUserEvents(userId);
                return Ok(events.Select(e => new EventEntry(e)).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpGet("next/{n}")]
        public async Task<ActionResult<List<EventEntry>>> GetNextEvents(int n)
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var events = await _eventService.GetNextEvents(userId, n);
                return Ok(events.Select(e => new EventEntry(e)).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventEntry>> GetEvent(int id)
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var eventEntity = await _eventService.GetEvent(userId, id);
                if (eventEntity == null)
                {
                    return NotFound(); // 404 Not Found
                }
                return Ok(new EventEntry(eventEntity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EventEntry>> CreateEvent(EventData eventData)
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var eventEntity = await _eventService.AddEvent(userId, eventData);
                return StatusCode(201, new EventEntry(eventEntity)); // 201 Created
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<EventEntry>> UpdateEvent(int id, EventData eventData)
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var eventEntity = await _eventService.UpdateEvent(userId, id, eventData);
                if (eventEntity == null)
                {
                    return NotFound(); // 404 Not Found
                }
                return Ok(new EventEntry(eventEntity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventEntry>> DeleteEvent(int id)
        {
            try
            {
                var userId = int.Parse(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                    .Value ?? throw new ArgumentException("Invalid token")
                );
                var eventEntity = await _eventService.DeleteEvent(userId, id);
                if (eventEntity == null)
                {
                    return NotFound(); // 404 Not Found
                }
                return Ok(new EventEntry(eventEntity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }
    }
}
