using System.ComponentModel.DataAnnotations;

namespace BETMart.BLL.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}