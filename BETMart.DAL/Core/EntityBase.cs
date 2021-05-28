using System;
using System.ComponentModel.DataAnnotations;

namespace BETMart.DAL.Core
{
    public abstract class EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(200)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
