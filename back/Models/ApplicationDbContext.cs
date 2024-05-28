using Microsoft.EntityFrameworkCore;

namespace back.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Internal.Task> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Internal.Task>()
                .HasKey(t => t.Id);
        }
    }
}
