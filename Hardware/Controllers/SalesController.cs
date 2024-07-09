using Hardware.Models;
using Microsoft.AspNetCore.Mvc;

public class SalesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Sales/Create
    public IActionResult Create()
    {
        ViewBag.Products = _context.Products.ToList();
        return View();
    }

    // POST: Sales/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,SaleDate,QuantitySold,UnitPrice")] Sale sale)
    {
        if (ModelState.IsValid)
        {
            _context.Add(sale);
            await _context.SaveChangesAsync();

            // Update Inventory
            var inventory = _context.Inventory.SingleOrDefault(i => i.ProductId == sale.ProductId);
            if (inventory != null)
            {
                inventory.UnitsSold += sale.QuantitySold;
                inventory.CurrentQuantity -= sale.QuantitySold;
            }
            else
            {
                inventory = new Inventory
                {
                    ProductId = sale.ProductId,
                    UnitsSold = sale.QuantitySold,
                    CurrentQuantity = -sale.QuantitySold // Initial sale without purchase
                };
                _context.Inventory.Add(inventory);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }
        ViewBag.Products = _context.Products.ToList();
        return View(sale);
    }
}
