using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.Product = _context.Products.ToList(); // Correctly access Products DbSet
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SaleDate,QuantitySold,UnitPrice")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(sale.ProductId); // Correctly access Products DbSet
                if (product != null)
                {
                    // Adjust inventory and sales
                    product.Quantity -= sale.QuantitySold;
                    product.UnitsSold += sale.QuantitySold;

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    _context.Add(sale);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Product = _context.Products.ToList(); // Reload products for the view if there's an error
            return View(sale);
        }
    }
}
