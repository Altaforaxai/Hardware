using System.ComponentModel.DataAnnotations;

namespace Hardware.Models
{
    public class Purchase
    {
        [Key]
        public int? PurchaseId { get; set; }
        [Required(ErrorMessage = "Product is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Purchase date is required.")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        [Required(ErrorMessage = "Quantity purchased is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity purchased must be at least 1.")]
        public int QuantityPurchased { get; set; }
        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
        public string? name { get; set; }

        // Navigation property
        public Product Product { get; set; }
        public Inventory Inventory { get; set; }

    }
}
