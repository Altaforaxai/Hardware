namespace Hardware.Models.custommodel
{
    public class customproduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? UnitsSold { get; set; }
        public int? UnitsPurchased { get; set; }
    }
}
