using back.Models;
using back.Models.Internal;
using back.Models.Transfer;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace back.Services
{
    public interface IAuthService
    {
        public Task RegisterUser(Models.Transfer.UserCredentials user);
        public Task<Models.Internal.User> GetUser(Models.Transfer.UserCredentials user);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
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
    }
}