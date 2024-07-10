using System.ComponentModel.DataAnnotations;

namespace Hardware.Models
{
    public class Inventory
    {
        [Key] // Define a primary key for Inventory

        public int ProductId { get; set; }
        public int PurchaseId { get; set; }

        public int CurrentQuantity { get; set; }
        public int UnitsSold { get; set; }
        public int UnitsPurchased { get; set; }

        public Product Product { get; set; }
        public Purchase Purchase { get; set; }
    }
}

