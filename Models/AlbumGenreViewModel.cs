using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlbumRegister.Models;

public class TicketListViewModel
{
    public List<Ticket>? Tickets { get; set; }
    public SelectList? Statuses { get; set; }
    public SelectList? Priorities { get; set; }
    public string? FilterStatus { get; set; }
    public string? FilterPriority { get; set; }
    public string? SearchString { get; set; }

    public PagingInfo PagingInfo { get; set; } = new();
}