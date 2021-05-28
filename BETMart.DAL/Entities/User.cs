using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    public class User
        : EntityBase
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]

        public string Password { get; set; }
    }

    public class Customer
        : User
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Phone]
        [MaxLength(13)]
        public string Phone { get; set; }
        [Phone]
        [MaxLength(13)]
        public string Mobile { get; set; }
        public Address Address { get; set; }
        public BillingInformation BillingInformation { get; set; }
    }
    
    public class Address
        : EntityBase
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; }
        [Required]
        [MaxLength(200)]
        public string AddressLine2 { get; set; }
        [Required]
        public string AddressLine3 { get; set; }
        [Required]
        [MaxLength(100)]
        public string Town { get; set; }
        [Required]
        [MaxLength(100)]
        public string Province { get; set; }
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }

    public class BillingInformation
    {
        [Key]
        public int BillingInformationId { get; set; }
        public string Name { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }

    public class Product
        : EntityBase
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public byte[] Image { get; set; }

    }
    public class Order
        : EntityBase
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }

        [NotMapped]
        public decimal Total => this.OrderDetails.Sum(orderDetail => (orderDetail.Price * orderDetail.Quantity));

        public OrderStatus OrderStatus { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
    public class OrderDetail
        : EntityBase
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
    public class OrderHistory
        : EntityBase
    {
        public int OrderHistoryId { get; set; }


    }

    public enum OrderStatus
    {
        New,
        Pending,
        OnDelivery,
        Complete,
        Cancelled
    }


}
