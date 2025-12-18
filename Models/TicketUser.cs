using System.ComponentModel.DataAnnotations;

namespace AlbumRegister.Models;

public class TicketUser
{
    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [Display(Name = "Assigned Date")]
    [DataType(DataType.Date)]
    public DateTime AssignedDate { get; set; } = DateTime.Now;
}
