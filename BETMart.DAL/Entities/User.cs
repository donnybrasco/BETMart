using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BETMart.DAL.Entities
{
    [Table(nameof(User))]
    public class User
        : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool IsActive { get; set; } = false;
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
