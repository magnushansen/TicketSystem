namespace AlbumRegister.Models;

public class DashboardViewModel
{
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public int InProgressTickets { get; set; }
    public int CompletedTickets { get; set; }
    public int ClosedTickets { get; set; }
    public int TotalUsers { get; set; }
    public int CriticalPriorityTickets { get; set; }
    public int HighPriorityTickets { get; set; }
    public List<Ticket> RecentTickets { get; set; } = new List<Ticket>();
    public List<Ticket> UpcomingDueTickets { get; set; } = new List<Ticket>();
}
