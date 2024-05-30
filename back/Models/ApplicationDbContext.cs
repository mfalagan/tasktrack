using Microsoft.EntityFrameworkCore;

namespace back.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Internal.Task> Tasks { get; set; }
        public DbSet<Models.Internal.Token> Tokens { get; set; }
        public DbSet<Models.Internal.User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Internal.Task>()
                .HasKey(tsk => tsk.Id);

            modelBuilder.Entity<Models.Internal.Token>()
                .HasKey(tkn => tkn.Id);

            modelBuilder.Entity<Models.Internal.User>()
                .HasKey(usr => usr.Id);
        }
    }
}
