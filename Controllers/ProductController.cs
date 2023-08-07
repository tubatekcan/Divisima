using BL.Repositories;
using DAL.Entities;
using Divisima.Models;
using Divisima.Tools;
using Divisima.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Divisima.Controllers
{
    public class ProductController : Controller
    {
        IRepository<Product> repoProduct;
        public ProductController(IRepository<Product> _repoProduct)
        {
            repoProduct = _repoProduct;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/detay/{name}-{id}")]
        public IActionResult Detail(string name,int id)
        {
            Product product = repoProduct.GetAll(x => x.ID == id).Include(x => x.ProductPictures).Include(x => x.Brand).FirstOrDefault();
            ProductVM productVM = new() {
                Product = product,
                RelatedProducts = repoProduct.GetAll(x => x.BrandID == product.BrandID && x.ID != product.ID).Include(x => x.ProductPictures).Include(x => x.Brand)
            };
            return View(productVM);
        }

        [Route("/urun/ara")]
        public IActionResult GetSearchProduct(string search)
        {
            return Json(repoProduct.GetAll(x=>x.Name.ToLower().Contains(search.ToLower()) || x.Description.ToLower().Contains(search.ToLower())).Include(i=>i.ProductPictures).Select(s=>new SearchProduct {Name=s.Name,Picture=s.ProductPictures.FirstOrDefault().Picture,Link="/detay/"+GeneralTool.URLConvert(s.Name)+"-"+s.ID }));
        }
    }
}
