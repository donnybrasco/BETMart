using System.Linq;
using BETMart.BLL.Models;

namespace BETMart
{
    public class ShoppingCartViewModel
    {
        public Order Current { get; set; }
        public int? TotalProducts => this.Current?.OrderDetails.Count;
        public decimal? TotalCost => this.Current?.OrderDetails.Sum(x => x.Total);

        public string ErrorMessage { get; set; }
    }
}
