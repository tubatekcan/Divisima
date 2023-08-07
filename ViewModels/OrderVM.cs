using DAL.Entities;
using Divisima.Models;

namespace Divisima.ViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
}
