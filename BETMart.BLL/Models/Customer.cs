using System;
using System.ComponentModel.DataAnnotations;

namespace BETMart.BLL.Models
{
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
        //Address Information
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
        //Billing Information
        public string Name { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        //Audit
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}