using Microsoft.EntityFrameworkCore;
using AlbumRegister.Data;


namespace AlbumRegister.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new TicketContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<TicketContext>>()))
        {
            if (context.Tickets.Any() || context.Users.Any())
            {
                return;
            }

            // Create users
            User u1 = new User { Username = "jdoe", Email = "jdoe@example.com", FullName = "John Doe" };
            User u2 = new User { Username = "asmith", Email = "asmith@example.com", FullName = "Alice Smith" };
            User u3 = new User { Username = "bjones", Email = "bjones@example.com", FullName = "Bob Jones" };
            User u4 = new User { Username = "cwhite", Email = "cwhite@example.com", FullName = "Carol White" };
            User u5 = new User { Username = "dbrowndev", Email = "dbrown@example.com", FullName = "David Brown" };

            context.Users.AddRange(u1, u2, u3, u4, u5);

            // Create tickets
            Ticket t1 = new Ticket
            {
                Title = "Fix login page bug",
                Description = "Users are unable to log in when using special characters in password",
                Status = "In Progress",
                Priority = "High",
                CreatedDate = DateTime.Parse("2025-01-15"),
                DueDate = DateTime.Parse("2025-01-25")
            };

            Ticket t2 = new Ticket
            {
                Title = "Add dark mode support",
                Description = "Implement dark mode theme throughout the application",
                Status = "Open",
                Priority = "Medium",
                CreatedDate = DateTime.Parse("2025-01-18"),
                DueDate = DateTime.Parse("2025-02-15")
            };

            Ticket t3 = new Ticket
            {
                Title = "Database performance optimization",
                Description = "Optimize slow queries in the reporting module",
                Status = "Open",
                Priority = "Critical",
                CreatedDate = DateTime.Parse("2025-01-20"),
                DueDate = DateTime.Parse("2025-01-30")
            };

            Ticket t4 = new Ticket
            {
                Title = "Update user documentation",
                Description = "Refresh the user manual with new features from v2.0",
                Status = "Completed",
                Priority = "Low",
                CreatedDate = DateTime.Parse("2025-01-10"),
                DueDate = DateTime.Parse("2025-01-20")
            };

            Ticket t5 = new Ticket
            {
                Title = "Implement export to PDF feature",
                Description = "Add ability to export reports as PDF files",
                Status = "In Progress",
                Priority = "Medium",
                CreatedDate = DateTime.Parse("2025-01-16"),
                DueDate = DateTime.Parse("2025-02-05")
            };

            Ticket t6 = new Ticket
            {
                Title = "Security audit of authentication module",
                Description = "Conduct comprehensive security review and fix vulnerabilities",
                Status = "Open",
                Priority = "Critical",
                CreatedDate = DateTime.Parse("2025-01-22"),
                DueDate = DateTime.Parse("2025-02-01")
            };

            context.Tickets.AddRange(t1, t2, t3, t4, t5, t6);
            context.SaveChanges();

            // Create ticket-user assignments
            context.TicketUsers.AddRange(
                new TicketUser { Ticket = t1, User = u1, AssignedDate = DateTime.Parse("2025-01-15") },
                new TicketUser { Ticket = t1, User = u2, AssignedDate = DateTime.Parse("2025-01-16") },
                new TicketUser { Ticket = t2, User = u3, AssignedDate = DateTime.Parse("2025-01-18") },
                new TicketUser { Ticket = t3, User = u2, AssignedDate = DateTime.Parse("2025-01-20") },
                new TicketUser { Ticket = t3, User = u5, AssignedDate = DateTime.Parse("2025-01-20") },
                new TicketUser { Ticket = t4, User = u4, AssignedDate = DateTime.Parse("2025-01-10") },
                new TicketUser { Ticket = t5, User = u1, AssignedDate = DateTime.Parse("2025-01-16") },
                new TicketUser { Ticket = t5, User = u3, AssignedDate = DateTime.Parse("2025-01-17") },
                new TicketUser { Ticket = t6, User = u2, AssignedDate = DateTime.Parse("2025-01-22") },
                new TicketUser { Ticket = t6, User = u5, AssignedDate = DateTime.Parse("2025-01-22") }
            );

            context.SaveChanges();
        }
    }
}