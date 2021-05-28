using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    [Table(nameof(Product))]
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
}