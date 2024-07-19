using System.ComponentModel.DataAnnotations;

namespace Hardware.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }
        [Required(ErrorMessage = "Product is required.")]
        public int? ProductId { get; set; }
        [Required(ErrorMessage = "Sale date is required.")]
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; }
        [Required(ErrorMessage = "Quantity sold is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity sold must be at least 1.")]
        public int QuantitySold { get; set; }
        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public decimal UnitPrice { get; set; }
        public string? name { get; set; }

  
    }
}
