namespace Hardware.Models
{
    public class NewInventoryC
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }

        public string? Type { get; set; }
        public DateTime Date { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
