using Microsoft.AspNetCore.Mvc;
using AlbumRegister.Models;
using AlbumRegister.Data;

namespace AlbumRegister.Components;

public class TicketFeatured : ViewComponent
{
    private TicketContext context;

    public TicketFeatured(TicketContext ctx)
    {
        context = ctx;
    }

    public IViewComponentResult Invoke()
    {
        var Tickets = (IEnumerable<Ticket>)context.Tickets;
        Ticket? t = Tickets.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        return View("/Views/Shared/_TicketSummary.cshtml", t);
    }
}