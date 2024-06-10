using Microsoft.EntityFrameworkCore;

namespace back.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Internal.Token> Tokens { get; set; }
        public DbSet<Models.Internal.User> Users { get; set; }
        public DbSet<Models.Internal.Event> Events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Internal.Token>()
                .HasKey(tkn => tkn.Id);

            modelBuilder.Entity<Models.Internal.User>()
                .HasKey(usr => usr.Id);

            modelBuilder.Entity<Models.Internal.Event>()
                .HasKey(evt => evt.Id);

            modelBuilder.Entity<Models.Internal.Event>()
                .HasOne(evt => evt.Owner)
                .WithMany(usr => usr.Events)
                .HasForeignKey(evt => evt.UserId);

            modelBuilder.Entity<Models.Internal.Token>()
                .HasOne(tkn => tkn.User)
                .WithMany(usr => usr.Tokens)
                .HasForeignKey(tkn => tkn.UserId);
        }
    }
}
