namespace NorthwindAPI.Model
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public Int16? ReorderLevel { get; set; }
    }
}
