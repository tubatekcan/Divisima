using Azure.Core;
using BL.Repositories;
using DAL.Entities;
using Divisima.Models;
using Divisima.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Divisima.Controllers
{
    public class CartController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<City> repoCity;
        public CartController(IRepository<Product> _repoProduct, IRepository<City> _repoCity)
        {
            repoProduct = _repoProduct;
            repoCity = _repoCity;
        }

        public IActionResult Index()
        {
            CartVM cartVM = new()
            {
                Carts = JsonConvert.DeserializeObject<IEnumerable<Cart>>(Request.Cookies["MyCart"]),
                Products = repoProduct.GetAll().Include(x=>x.ProductPictures).OrderBy(x => Guid.NewGuid()).Take(4)
            };
            return View(cartVM);
        }

        [Route("/sepet/miktar")]
        public decimal[] GetCartCount()
        {
            decimal[] rtn = { 0, 0 };
            if (Request.Cookies["MyCart"] != null) { 

                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                rtn[0]= carts.Sum(x=>x.Quantity);
                rtn[1]= carts.Sum(x=>x.Quantity*x.ProductPrice);
            }
            return rtn;
        }

        [Route("/sepet/ekle"),HttpPost]
        public string AddCart(int productID,int quantity)
        {
            Product product = repoProduct.GetAll(x => x.ID == productID).Include(x=>x.ProductPictures).FirstOrDefault() ?? null;
            if (product != null)
            {
                Cart cart = new()
                {
                    ProductID = product.ID,
                    ProductName = product.Name,
                    ProductPicture = product.ProductPictures.FirstOrDefault().Picture,
                    ProductPrice = product.Price,
                    Quantity = quantity
                };
                List<Cart> carts = new();
                bool varmi = false;
                if (Request.Cookies["MyCart"] != null)
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                    foreach (Cart _cart in carts)
                    {
                        if (_cart.ProductID == productID)
                        {
                            varmi = true;
                            _cart.Quantity = quantity;
                            break;
                        }
                    }                    
                }

                //cookie: tarayıcıların bilgisayarımızda tuttukları metin dosyası
                if (varmi == false) carts.Add(cart);
                CookieOptions cookieOptions = new();
                cookieOptions.Expires = DateTime.Now.AddDays(3);
                Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), cookieOptions);
                return product.Name;
            }
            else return "";
        }

        [Route("/sepet/tamamla")]
        public IActionResult CheckOut()
        {
            OrderVM orderVM = new() { 
                Order=new Order(),
                Carts=JsonConvert.DeserializeObject<IEnumerable<Cart>>(Request.Cookies["MyCart"]),
                Cities=repoCity.GetAll().OrderBy(x=>x.Name)
            };
            return View(orderVM);
        }
    }
}
