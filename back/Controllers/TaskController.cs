using back.Services;
using back.Models.Transfer;
using back.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EjercicioApiRest.Controllers
{
    [Route("/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskDbService _service;

        public TaskController(ITaskDbService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("This is secured data");
        }

        // GET: /tasks
        [HttpGet]
        public async Task<ActionResult<List<TaskEntry>>> GetTasks()
        {
            try
            {
                var tasks = await _service.GetTasks();
                return Ok(tasks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        // GET: /tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntry>> GetTask(int id)
        {
            try
            {
                return Ok(await _service.GetTask(id));
            }
            catch (NotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        // POST: /tasks
        [HttpPost]
        public async Task<ActionResult<TaskEntry>> CreateTask(TaskData task)
        {
            try
            {
                return StatusCode(201, await _service.AddTask(task)); // 201 Created
            }
            catch (ArgumentException)
            {
                return StatusCode(400); // 400 Bad Request
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        // PUT: /tasks/
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskEntry>> UpdateTask(int id, TaskData task)
        {
            try
            {
                return StatusCode(201, await _service.UpdateTask(id, task)); // 201 Created
            }
            catch (NotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
            catch (ArgumentException)
            {
                return StatusCode(400); // 400 Bad Request
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }

        // DELETE: /tasks/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskData>> DeleteTask(int id)
        {
            try
            {
                return StatusCode(200, await _service.DeleteTask(id)); //200 OK
            }
            catch (NotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500); // 500 Internal Server Error
            }
        }
    }
}