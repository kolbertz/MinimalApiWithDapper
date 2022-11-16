namespace NorthwindAPI.Model
{
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string ProductName { get; set; }
        public List<string> SelectProperties { get; set; }

        public ProductFilter()
        {
            SelectProperties = new List<string>();
        }
    }
}
