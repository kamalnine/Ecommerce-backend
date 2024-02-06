using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemID { get; set; }

        public int OrderId { get; set; }
        public int signupId { get; set; }
        public int ProductID { get; set; }

       
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string ImageURL { get; set; }

        public string ProductName { get; set; }

        public string Variant { get; set; }
        public bool? Isactive { get; set; }
    }
}
