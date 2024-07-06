using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hardware.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewBag.Product = _context.Products.ToList(); // Correctly access Products DbSet
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,PurchaseDate,QuantityPurchased,UnitPrice")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(purchase.ProductId); // Correctly access Products DbSet
                if (product != null)
                {
                    // Adjust inventory and purchases
                    product.Quantity += purchase.QuantityPurchased;
                    product.UnitsPurchased += purchase.QuantityPurchased;

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    _context.Add(purchase);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Product = _context.Products.ToList(); // Reload products for the view if there's an error
            return View(purchase);
        }
    }
}
