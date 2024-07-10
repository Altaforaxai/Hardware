using Hardware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
                try
                {
                    using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                    {
                        await connection.OpenAsync();

                        using (var command = new SqlCommand("AddSale", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProductId", sale.ProductId);
                            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                            command.Parameters.AddWithValue("@QuantitySold", sale.QuantitySold);
                            command.Parameters.AddWithValue("@UnitPrice", sale.UnitPrice);

                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    return RedirectToAction("Index", "Inventory");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            ViewBag.Products = _context.Products.ToList();
            return View(sale);
        }
    }
}