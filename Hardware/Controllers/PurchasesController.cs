using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Create(int? id)
        {
            PopulateProductsDropDownList(id);

            return View();
        }
        private void PopulateProductsDropDownList(object selectedProduct = null)
        {
            var products = _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            ViewBag.Products = new SelectList(products, "Value", "Text", selectedProduct);
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

