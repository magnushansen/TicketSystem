using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlbumRegister.Models;
using AlbumRegister.Data;
using Microsoft.EntityFrameworkCore;

namespace AlbumRegister.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private TicketContext context;
    private IEnumerable<User> users => context.Users;
    public int PageSize = 10;

    public HomeController(ILogger<HomeController> logger, TicketContext data)
    {
        _logger = logger;
        context = data;
    }

    public IActionResult Ticket(string filterStatus, string filterPriority, string searchString, int ticketPage = 1)
    {
        var tickets = context.Tickets.Include(t => t.TicketUsers).ThenInclude(tu => tu.User).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            tickets = tickets.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!string.IsNullOrEmpty(filterStatus))
        {
            tickets = tickets.Where(x => x.Status == filterStatus);
        }

        if (!string.IsNullOrEmpty(filterPriority))
        {
            tickets = tickets.Where(x => x.Priority == filterPriority);
        }

        var totalItems = tickets.Count();

        var statuses = new List<string> { "Open", "In Progress", "Completed", "Closed" };
        var priorities = new List<string> { "Low", "Medium", "High", "Critical" };

        var ticketListVM = new TicketListViewModel
        {
            Statuses = new SelectList(statuses),
            Priorities = new SelectList(priorities),
            Tickets = tickets
                .OrderByDescending(t => t.CreatedDate)
                .Skip((ticketPage - 1) * PageSize)
                .Take(PageSize)
                .ToList(),
            FilterStatus = filterStatus,
            FilterPriority = filterPriority,
            SearchString = searchString,
            PagingInfo = new PagingInfo
            {
                CurrentPage = ticketPage,
                ItemsPerPage = PageSize,
                TotalItems = totalItems
            }
        };

        return View(ticketListVM);
    }

    public async Task<IActionResult> Details(int id)
    {
        Ticket? t = await context.Tickets
            .Include(t => t.TicketUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(m => m.TicketId == id);

        if (t == null)
        {
            return NotFound();
        }

        var assignedUsers = t.TicketUsers.Select(tu => tu.User!).ToList();
        TicketViewModel model = ViewModelFactory.Details(t, assignedUsers);
        return View("TicketEditor", model);
    }

    public IActionResult Create()
    {
        return View("TicketEditor",
            ViewModelFactory.Create(new() { Title = string.Empty }, users));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Ticket ticket, [FromForm] List<int> selectedUserIds)
    {
        if (ModelState.IsValid)
        {
            ticket.TicketId = default;
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                foreach (var userId in selectedUserIds)
                {
                    context.TicketUsers.Add(new TicketUser
                    {
                        TicketId = ticket.TicketId,
                        UserId = userId,
                        AssignedDate = DateTime.Now
                    });
                }
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Ticket));
        }
        return View("TicketEditor",
            ViewModelFactory.Create(ticket, users));
    }

    public async Task<IActionResult> Edit(int id)
    {
        Ticket? t = await context.Tickets
            .Include(t => t.TicketUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (t != null)
        {
            var assignedUsers = t.TicketUsers.Select(tu => tu.User!).ToList();
            var selectedUserIds = t.TicketUsers.Select(tu => tu.UserId).ToList();
            TicketViewModel model = ViewModelFactory.Edit(t, users, assignedUsers, selectedUserIds);
            return View("TicketEditor", model);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] Ticket ticket, [FromForm] List<int> selectedUserIds)
    {
        if (ModelState.IsValid)
        {
            context.Tickets.Update(ticket);

            var existingAssignments = context.TicketUsers.Where(tu => tu.TicketId == ticket.TicketId);
            context.TicketUsers.RemoveRange(existingAssignments);

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                foreach (var userId in selectedUserIds)
                {
                    context.TicketUsers.Add(new TicketUser
                    {
                        TicketId = ticket.TicketId,
                        UserId = userId,
                        AssignedDate = DateTime.Now
                    });
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Ticket));
        }

        var t = await context.Tickets
            .Include(t => t.TicketUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.TicketId == ticket.TicketId);

        var assignedUsers = t?.TicketUsers.Select(tu => tu.User!).ToList() ?? new List<User>();
        return View("TicketEditor",
            ViewModelFactory.Edit(ticket, users, assignedUsers, selectedUserIds ?? new List<int>()));
    }

    public async Task<IActionResult> Delete(int id)
    {
        Ticket? t = await context.Tickets
            .Include(t => t.TicketUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (t != null)
        {
            var assignedUsers = t.TicketUsers.Select(tu => tu.User!).ToList();
            TicketViewModel model = ViewModelFactory.Delete(t, assignedUsers);
            return View("TicketEditor", model);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Ticket ticket)
    {
        var assignments = context.TicketUsers.Where(tu => tu.TicketId == ticket.TicketId);
        context.TicketUsers.RemoveRange(assignments);

        context.Tickets.Remove(ticket);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Ticket));
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await context.Tickets
            .Include(t => t.TicketUsers)
            .ThenInclude(tu => tu.User)
            .ToListAsync();

        var totalUsers = await context.Users.CountAsync();

        var viewModel = new DashboardViewModel
        {
            TotalTickets = tickets.Count,
            OpenTickets = tickets.Count(t => t.Status == "Open"),
            InProgressTickets = tickets.Count(t => t.Status == "In Progress"),
            CompletedTickets = tickets.Count(t => t.Status == "Completed"),
            ClosedTickets = tickets.Count(t => t.Status == "Closed"),
            TotalUsers = totalUsers,
            CriticalPriorityTickets = tickets.Count(t => t.Priority == "Critical"),
            HighPriorityTickets = tickets.Count(t => t.Priority == "High"),
            RecentTickets = tickets.OrderByDescending(t => t.CreatedDate).Take(5).ToList(),
            UpcomingDueTickets = tickets
                .Where(t => t.DueDate.HasValue && t.DueDate.Value >= DateTime.Today && t.Status != "Completed" && t.Status != "Closed")
                .OrderBy(t => t.DueDate)
                .Take(5)
                .ToList()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
