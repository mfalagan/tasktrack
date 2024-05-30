using back.Models;
using back.Models.Internal;
using back.Models.Transfer;
using Microsoft.EntityFrameworkCore;

namespace back.Services
{
    public class TaskDbService : ITaskDbService
    {
        private readonly ApplicationDbContext _context;

        public TaskDbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Models.Transfer.UserCredentials> RegisterUser(Models.Transfer.UserCredentials user)
        {
            throw new NotImplementedException(); // TODO
        }

        public Task<User> GetUser(UserCredentials user)
        {
            throw new NotImplementedException(); //TODO
        }

        public async Task<List<TaskEntry>> GetTasks()
        {
            return await _context.Tasks.Select(t => new TaskEntry(t)).ToListAsync();
        }

        public async Task<TaskEntry> GetTask(int id)
        {
            var result = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (result == null)
                throw new Exceptions.NotFoundException();
            else
                return new TaskEntry(result);
        }

        public async Task<TaskEntry> AddTask(TaskData task)
        {
            var newTask = new Models.Internal.Task(task);
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return new TaskEntry(newTask);
        }

        public async Task<TaskEntry> UpdateTask(int id, TaskData task)
        {
            var task_cur = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task_cur == null)
                throw new Exceptions.NotFoundException();
            task_cur.Name = task.Name;
            task_cur.DueDate = task.DueDate;
            task_cur.Description = task.Description;
            await _context.SaveChangesAsync();

            return new TaskEntry(task_cur);
        }

        public async Task<TaskData> DeleteTask(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                throw new Exceptions.NotFoundException();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return new TaskData(task);
        }
    }
}