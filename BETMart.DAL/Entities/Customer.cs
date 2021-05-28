using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETMart.DAL.Entities
{
    [Table(nameof(Customer))]
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
}