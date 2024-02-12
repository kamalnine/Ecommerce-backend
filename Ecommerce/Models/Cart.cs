using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Cart
    {
        public int CartID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string Variant { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }

        public decimal TotalPrice { get; set; }
        public bool? Isactive { get; set; }
    }
}
