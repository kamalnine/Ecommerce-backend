﻿namespace Ecommerce.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public bool? Isactive { get; set; }
    }
}
