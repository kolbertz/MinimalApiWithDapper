using System.ComponentModel.DataAnnotations;

namespace NorthwindAPI.Model
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public Int16? ReorderLevel { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public bool Discontinued { get; set; }

        public Products() { }

        public Products(ProductCreateDTO createDTO)
        {
            if (createDTO != null)
            {
                ProductName = createDTO.ProductName;
                SupplierID = createDTO.SupplierID;
                CategoryID = createDTO.CategoryID;
                QuantityPerUnit = createDTO.QuantityPerUnit;
                UnitPrice = createDTO.UnitPrice;
                ReorderLevel = createDTO.ReorderLevel;
            }
        }
    }
}
