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
        ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,PurchaseDate,QuantityPurchased,UnitPrice")] Purchase purchase)
        {
        if (!ModelState.IsValid)
            {
            _context.Add(purchase);
                    await _context.SaveChangesAsync();

            // Update Inventory
            var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.ProductId == purchase.ProductId);
            if (inventory != null)
            {
                inventory.UnitsPurchased += purchase.QuantityPurchased;
                inventory.CurrentQuantity += purchase.QuantityPurchased;
            }
            else
            {
                inventory = new Inventory
                {
                    ProductId = purchase.ProductId,
                    UnitsPurchased = purchase.QuantityPurchased,
                    CurrentQuantity = purchase.QuantityPurchased
                };
                _context.Inventory.Add(inventory);
            }
                    await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
            }
        ViewBag.Products = _context.Products.ToList();
            return View(purchase);
        }

}

