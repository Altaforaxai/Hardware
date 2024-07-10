namespace Hardware.Models
{
    public class Purchase
    {
        public int? PurchaseId { get; set; }
        public int ProductId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal UnitPrice { get; set; }
        public string? name { get; set; }

        // Navigation property
        public Product Product { get; set; }
        public Inventory Inventory { get; set; }
    }
}
