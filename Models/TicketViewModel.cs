namespace AlbumRegister.Models;

public class TicketViewModel
{
    public Ticket Ticket { get; set; }
        = new() { Title = string.Empty };
    public string Action { get; set; } = "Create";
    public bool ReadOnly { get; set; } = false;
    public string Theme { get; set; } = "primary";
    public bool ShowAction { get; set; } = true;
    public IEnumerable<User> Users { get; set; }
        = Enumerable.Empty<User>();
    public IEnumerable<User> AssignedUsers { get; set; }
        = Enumerable.Empty<User>();
    public List<int> SelectedUserIds { get; set; } = new List<int>();
}
