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
    public async Task<IActionResult> DeleteAll()
    {
        // Delete all rows from the Inventory table
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Inventory");

        // Redirect to a suitable action, like Index
        return RedirectToAction("Index", "Product"); // Redirect to home page or any other appropriate action
    }
    // GET: Inventory
    public async Task<IActionResult> Index()
    {
        var purchases = await _context.Purchases
                .Select(p => new NewInventoryC
                {
                    Name = p.name,

                    ProductId = p.ProductId,
                    Type = "Purchase",
                    Date = p.PurchaseDate,
                    Quantity = p.QuantityPurchased,
                    UnitPrice = p.UnitPrice
                })
                .ToListAsync();

        var sales = await _context.Sales
            .Select(s => new NewInventoryC
            {
                Name = s.name,
                ProductId = s.ProductId,
                Type = "Sale",
                Date = s.SaleDate,
                Quantity = s.QuantitySold,
                UnitPrice = s.UnitPrice
            })
            .ToListAsync();

        var NewInventoryC = purchases.Concat(sales)
            .OrderByDescending(e => e.Date)
            .ToList();

        return View(NewInventoryC);
    }
}


    // GET: Inventory/Details/5
    //public async Task<IActionResult> Details(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var inventory = await _context.Inventory
    //        .Include(i => i.Product)
    //        .FirstOrDefaultAsync(m => m.ProductId == id);
    //    if (inventory == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(inventory);
    //}

    // GET: Inventory/Create
    //public IActionResult Create()
    //{
    //    ViewBag.Products = _context.Products.ToList();
    //    return View();
    //}

    //// POST: Inventory/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("ProductId,CurrentQuantity,UnitsSold,UnitsPurchased")] Inventory inventory)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Add(inventory);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewBag.Products = _context.Products.ToList();
    //    return View(inventory);
    //}

    //// GET: Inventory/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var inventory = await _context.Inventory.FindAsync(id);
    //    if (inventory == null)
    //    {
    //        return NotFound();
    //    }
    //    ViewBag.Products = _context.Products.ToList();
    //    return View(inventory);
    //}

    //// POST: Inventory/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("ProductId,CurrentQuantity,UnitsSold,UnitsPurchased")] Inventory inventory)
    //{
    //    if (id != inventory.ProductId)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            _context.Update(inventory);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!InventoryExists(inventory.ProductId))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewBag.Products = _context.Products.ToList();
    //    return View(inventory);
    //}

    // GET: Inventory/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var inventory = await _context.Inventory
    //        .Include(i => i.Product)
    //        .FirstOrDefaultAsync(m => m.ProductId == id);
    //    if (inventory == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(inventory);
    //}

    // POST: Inventory/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var inventory = await _context.Inventory.FindAsync(id);
    //    _context.Inventory.Remove(inventory);
    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //private bool InventoryExists(int id)
    //{
    //    return _context.Inventory.Any(e => e.ProductId == id);
    //}
//}
