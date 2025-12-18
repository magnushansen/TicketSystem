using System.ComponentModel.DataAnnotations;

namespace AlbumRegister.Models;

public class Ticket
{
    public int TicketId { get; set; }

    [StringLength(100, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }

    [Required]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Required]
    [StringLength(20)]
    public string? Status { get; set; } = "Open";

    [Required]
    [StringLength(20)]
    public string? Priority { get; set; } = "Medium";

    [Display(Name = "Created Date")]
    [DataType(DataType.Date)]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    public ICollection<TicketUser> TicketUsers { get; set; } = new List<TicketUser>();
}
