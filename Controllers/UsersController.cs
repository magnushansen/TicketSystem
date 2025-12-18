using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlbumRegister.Data;
using AlbumRegister.Models;

namespace AlbumRegister.Controllers;

public class UsersController : Controller
{
    private readonly TicketContext _context;
    public int PageSize = 10;

    public UsersController(TicketContext context)
    {
        _context = context;
    }

    // GET: Users
    public async Task<IActionResult> Index(int userPage = 1)
    {
        var users = _context.Users.AsQueryable();
        var totalItems = await users.CountAsync();

        var viewModel = new UserListViewModel
        {
            Users = await users
                .OrderBy(u => u.UserId)
                .Skip((userPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync(),
            PagingInfo = new PagingInfo
            {
                CurrentPage = userPage,
                ItemsPerPage = PageSize,
                TotalItems = totalItems
            }
        };

        return View(viewModel);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .Include(u => u.TicketUsers)
            .ThenInclude(tu => tu.Ticket)
            .FirstOrDefaultAsync(m => m.UserId == id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserId,Username,Email,FullName")] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,FullName")] User user)
    {
        if (id != user.UserId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.UserId == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
}
