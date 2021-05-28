using System.ComponentModel.DataAnnotations.Schema;
using BETMart.DAL.Core;

namespace BETMart.DAL.Entities
{
    [Table(nameof(OrderHistory))]
    public class OrderHistory
        : EntityBase
    {
        public int OrderHistoryId { get; set; }


    }
}