using back.Models;
using back.Models.Internal;
using back.Models.Transfer;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace back.Services
{
    public class TaskDbService : ITaskDbService
    {
        private readonly ApplicationDbContext _context;

        public TaskDbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(Models.Transfer.UserCredentials user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Username == null && user.Email == null)
                throw new ArgumentException("Email or Username must be provided");

            if (user.Password == null)
                throw new ArgumentException("Password must be provided");

            if (user.Username != null)
            {
                if (_context.Users.Any(u => u.Username == user.Username))
                    throw new ArgumentException("Username already in use");
            }
            if (user.Email != null)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                    throw new ArgumentException("Email already in use");
            }

            _context.Users.Add(new User(user.Username, user.Email, user.GetValidPassword()));
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(UserCredentials user)
        {
            if (user.Username == null && user.Email == null)
                throw new ArgumentException("Email or Username must be provided");
            
            var users = _context.Users.Where(u => true);

            if (user.Username != null)
                users = users.Where(u => u.Username == user.Username);

            if (user.Email != null)
                users = users.Where(u => u.Email == user.Email);

            if (await users.CountAsync() > 1)
                throw new ArgumentException("Multiple users found");
            
            try {
                return await users.FirstAsync();
            } catch (InvalidOperationException) {
                throw new ArgumentException("User not found");
            }
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