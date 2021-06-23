using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BETMart.Common;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    [Table(nameof(Order))]
    public class Order
        : EntityBase
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string OrderNumber { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }

        [NotMapped]
        public decimal Total => this.OrderDetails.Sum(orderDetail => (orderDetail.Price * orderDetail.Quantity));

        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}