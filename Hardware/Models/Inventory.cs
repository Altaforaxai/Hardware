namespace Hardware.Models
{
    public class Inventory
    {
        public int ProductId { get; set; }
        public int CurrentQuantity { get; set; }
        public int UnitsSold { get; set; }
        public int UnitsPurchased { get; set; }

        public Product Product { get; set; }
    }
}

