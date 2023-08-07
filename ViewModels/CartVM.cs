using DAL.Entities;
using Divisima.Models;

namespace Divisima.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> Carts { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
