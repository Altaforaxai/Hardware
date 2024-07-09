using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Hardware.Models;

public class InventoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Inventory
    public async Task<IActionResult> Index()
    {
        var inventory = await _context.Inventory.Include(i => i.Purchase).ToListAsync();
        return View(inventory);
    }
}
