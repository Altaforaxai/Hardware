using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Hardware.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hardware.Models.custommodel;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Product/Index
    public  IActionResult Index()
    {       
        
        

   

        var ur = (from u in  _context.Products
                  join r in  _context.ProductCategories on u.ProductCategoryId equals r.Id

                  select new Product
                  {

                      Id = u.Id,
                      Name = u.Name,
                      Price = u.Price,
                      ProductName = r.ProductName,
                      Quantity = u.Quantity,
                      UnitsPurchased = u.UnitsPurchased,
                      UnitsSold = u.UnitsSold,
                  }).ToList();


        return View(ur); // Ensure you're accessing Products correctly
    }

    public async Task<IActionResult> Search(string searchString)
    {
        var products = from p in _context.Products
                       select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(s => s.Name.Contains(searchString));
        }

        return View("Index", await products.ToListAsync());
    }


    // GET: Product/Details/5

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Where(p => p.Id==id)
            .FirstOrDefaultAsync();
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }




    // GET: Product/Create
    public IActionResult Create()
    {
        // Create a list of predefined categories
        var categories = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Iron" },
        new SelectListItem { Value = "2", Text = "Steel" },
        new SelectListItem { Value = "3", Text = "Plastic" },
        new SelectListItem { Value = "4", Text = "Glass" }
    };

        // Pass the list to the view
        ViewBag.ProductCategories = categories;

        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,ProductCategoryId,Quantity,UnitsPurchased,UnitsSold")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }



    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        PopulateCategoriesDropDownList(product.ProductName);
        return View(product);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProductCategoryId,Price,ProductName,Quantity,UnitsPurchased,UnitsSold")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        PopulateCategoriesDropDownList(product.ProductName);
        return View(product);
    }
    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
    private void PopulateCategoriesDropDownList(object selectedCategory = null)
    {
        var categories = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Iron" },
            new SelectListItem { Value = "2", Text = "Steel" },
            new SelectListItem { Value = "3", Text = "Plastic" },
            new SelectListItem { Value = "4", Text = "Glass" }
        };

        ViewBag.ProductCategories = new SelectList(categories, "Value", "Text", selectedCategory);
    }
}
