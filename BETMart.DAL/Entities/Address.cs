using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    [Table(nameof(Address))]
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

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}