using System;
using System.ComponentModel.DataAnnotations;

namespace BETMart.BLL.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}