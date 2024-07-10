using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        var inventories = await _context.Inventories.Include(i => i.Product).ToListAsync();
        return View(inventories);
    }

    // GET: Inventory/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inventory = await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (inventory == null)
        {
            return NotFound();
        }

        return View(inventory);
    }

    // GET: Inventory/Create
    public IActionResult Create()
    {
        ViewBag.Products = _context.Products.ToList();
        return View();
    }

    // POST: Inventory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,CurrentQuantity,UnitsSold,UnitsPurchased")] Inventory inventory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Products = _context.Products.ToList();
        return View(inventory);
    }

    // GET: Inventory/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null)
        {
            return NotFound();
        }
        ViewBag.Products = _context.Products.ToList();
        return View(inventory);
    }

    // POST: Inventory/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductId,CurrentQuantity,UnitsSold,UnitsPurchased")] Inventory inventory)
    {
        if (id != inventory.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(inventory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(inventory.ProductId))
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
        ViewBag.Products = _context.Products.ToList();
        return View(inventory);
    }

    // GET: Inventory/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inventory = await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (inventory == null)
        {
            return NotFound();
        }

        return View(inventory);
    }

    // POST: Inventory/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool InventoryExists(int id)
    {
        return _context.Inventories.Any(e => e.ProductId == id);
    }
}
