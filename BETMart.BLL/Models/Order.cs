using System;
using System.Collections.Generic;

namespace BETMart.BLL.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}