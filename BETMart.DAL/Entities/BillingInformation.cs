using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BETMart.DAL.Entities
{
    [Table(nameof(BillingInformation))]
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
}