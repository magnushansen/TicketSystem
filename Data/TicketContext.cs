using Microsoft.EntityFrameworkCore;
using AlbumRegister.Models;

namespace AlbumRegister.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TicketUser> TicketUsers => Set<TicketUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<TicketUser>()
                .HasKey(tu => new { tu.TicketId, tu.UserId });

            modelBuilder.Entity<TicketUser>()
                .HasOne(tu => tu.Ticket)
                .WithMany(t => t.TicketUsers)
                .HasForeignKey(tu => tu.TicketId);

            modelBuilder.Entity<TicketUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.TicketUsers)
                .HasForeignKey(tu => tu.UserId);
        }
    }
}
