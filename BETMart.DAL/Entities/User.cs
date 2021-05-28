using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    [Table(nameof(User))]
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
}
