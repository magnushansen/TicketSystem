using System.ComponentModel.DataAnnotations;

namespace AlbumRegister.Models;

public class User
{
    public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string? FullName { get; set; }

    public ICollection<TicketUser> TicketUsers { get; set; } = new List<TicketUser>();
}
