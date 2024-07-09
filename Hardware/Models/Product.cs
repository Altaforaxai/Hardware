namespace Hardware.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ProductCategoryId { get; set; }
        public int Quantity { get; set; } 
        public int UnitsSold { get; set; } 
        public int UnitsPurchased { get; set; } 

        // Navigation properties
        public ProductCategory ProductCategory { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
