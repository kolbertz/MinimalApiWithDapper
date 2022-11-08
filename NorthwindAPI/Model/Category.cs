using System.ComponentModel.DataAnnotations;

namespace NorthwindAPI.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
