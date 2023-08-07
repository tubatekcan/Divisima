using BL.Repositories;
using DAL.Entities;
using Divisima.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Divisima.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Slide> repoSlide;
        IRepository<Product> repoProduct;
        public HomeController(IRepository<Slide> _repoSlide, IRepository<Product> _repoProduct)
        {
            repoSlide = _repoSlide;
            repoProduct = _repoProduct;
        }
        public IActionResult Index()
        {
            IndexVM indexVM = new() {
                Slides = repoSlide.GetAll().OrderBy(x => x.DisplayIndex),
                LatestProducts = repoProduct.GetAll().Include(x=>x.ProductPictures).OrderByDescending(x => x.ID).Take(10),
                TopSellingProducts = repoProduct.GetAll().Include(x => x.ProductPictures).OrderBy(x=>Guid.NewGuid()).Take(8)
            };
            return View(indexVM);
        }

      
    }
}
