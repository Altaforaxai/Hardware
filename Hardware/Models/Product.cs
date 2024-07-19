using System.ComponentModel.DataAnnotations;

namespace Hardware.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Product Category is required")]
        public int? ProductCategoryId { get; set; }
        public string? ProductName { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number")]
        public int? Quantity { get; set; } 
        public int? UnitsSold { get; set; } 
        public int? UnitsPurchased { get; set; } 

        // Navigation properties
        //public ProductCategory ProductCategory { get; set; }
        //public ICollection<Sale> Sales { get; set; }
        //public ICollection<Purchase> Purchases { get; set; }
    }
}
