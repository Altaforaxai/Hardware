using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
                using (var connection = (SqlConnection)_context.Database.GetDbConnection())
                {
                    connection.Open();

                    using (var command = new SqlCommand("AddPurchase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", purchase.ProductId);
                        command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);
                        command.Parameters.AddWithValue("@QuantityPurchased", purchase.QuantityPurchased);
                        command.Parameters.AddWithValue("@UnitPrice", purchase.UnitPrice);

                        await command.ExecuteNonQueryAsync();
                    }

                    connection.Close();
                }

                return RedirectToAction("Index", "Inventory");
            }
            ViewBag.Products = _context.Products.ToList();
            return View(purchase);
        }

    }
}

