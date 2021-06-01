using System.ComponentModel.DataAnnotations;

namespace BETMart.BLL.Security
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}