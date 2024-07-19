using Microsoft.AspNetCore.Mvc;
using Hardware.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class InventoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAll()
    {
        // Delete all rows from the Inventory table
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Inventory");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Purchases");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Sales");

        // Redirect to the inventory history view or any other appropriate action
        return RedirectToAction("Index");
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
