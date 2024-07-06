using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hardware.Controllers
{
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
            ViewBag.Products = _context.Products.ToList(); // Load products for dropdown
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SaleDate,QuantitySold,UnitPrice")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product != null && product.Quantity >= sale.QuantitySold)
                {
                    // Start transaction to ensure atomicity
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Adjust inventory and sales
                            product.Quantity -= sale.QuantitySold;
                            product.UnitsSold += sale.QuantitySold;

                            _context.Update(product);
                            await _context.SaveChangesAsync();

                            // Save the sale record
                            sale.SaleDate = DateTime.Now; // Set sale date to current date/time
                            _context.Add(sale);
                            await _context.SaveChangesAsync();

                            // Update or add Inventory record
                            var inventory = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == sale.ProductId);
                            if (inventory != null)
                            {
                                inventory.CurrentQuantity -= sale.QuantitySold;
                                inventory.UnitsSold += sale.QuantitySold;
                                _context.Update(inventory);
                            }
                            else
                            {
                                // Create a new inventory record if it doesn't exist
                                inventory = new Inventory
                                {
                                    ProductId = sale.ProductId,
                                    CurrentQuantity = product.Quantity,
                                    UnitsSold = sale.QuantitySold,
                                    UnitsPurchased = product.UnitsPurchased
                                };
                                _context.Add(inventory);
                            }
                            await _context.SaveChangesAsync();

                            transaction.Commit(); // Commit transaction if everything succeeds

                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Rollback transaction on error
                            ModelState.AddModelError("", "Failed to save the sale. Please try again."); // Add error message to model state
                            ViewBag.Products = _context.Products.ToList(); // Reload products for the view
                            return View(sale);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Not enough quantity available for the selected product."); // Add error message to model state
                    ViewBag.Products = _context.Products.ToList(); // Reload products for the view
                    return View(sale);
                }
            }

            ViewBag.Products = _context.Products.ToList(); // Reload products for the view if there's an error
            return View(sale);
        }

        // GET: Sales/Index
        public async Task<IActionResult> Index()
        {
            var sales = await _context.Sales.Include(s => s.Product).ToListAsync();
            return View(sales);
        }
    }
}
