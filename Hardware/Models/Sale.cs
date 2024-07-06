namespace Hardware.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public DateTime SaleDate { get; set; }
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation property
        public Product Product { get; set; }
    }
}
